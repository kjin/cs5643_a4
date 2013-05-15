using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Common;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Double.Solvers;

namespace Fluid
{
    public class BasicFluid2D : GLDrawable
    {
        //(2 dimensions) with velocity
        double[,] u_x, u_y;
        GridCell[,] cells;
        int max_x, max_y;
        double timestep = FluidConstants.DEFAULT_TIMESTEP;
        public Vector2 GlobalForce;

        SparseMatrix A;
        MathNet.Numerics.LinearAlgebra.Double.Solvers.Preconditioners.IPreConditioner solver;
        Vector p, d;

        //threading stuff
        volatile bool _running = false;

        public BasicFluid2D(int max_x, int max_y)
        {
            if (max_x != max_y) throw new ArgumentException();
            this.max_x = max_x;
            this.max_y = max_y;

            u_x = new double[max_x+1,max_y];
            u_y = new double[max_x,max_y+1];
            cells = new GridCell[max_x, max_y];
            A = new SparseMatrix(max_x * max_y);
            for (int i = 0; i < max_x; i++)
                for (int j = 0; j < max_y; j++)
                {
                    A[i * max_x + j, i * max_x + j] = 2 + (i == 0 || i + 1 == max_x ? 1 : 0) + (j == 0 || j + 1 == max_y ? 1 : 0);
                    if (i > 0)
                        A[(i - 1) * max_x + j, i * max_x + j] = A[i * max_x + j, (i - 1) * max_x + j] = -1;
                    if (j > 0)
                        A[i * max_x + j - 1, i * max_x + j] = A[i * max_x + j , i * max_x + j - 1] = -1;
                    if (i < max_x - 1)
                        A[(i + 1) * max_x + j, i * max_x + j] = A[i * max_x + j, (i + 1) * max_x + j] = -1;
                    if (j < max_y - 1)
                        A[i * max_x + j + 1, i * max_x + j] = A[i * max_x + j, i * max_x + j + 1] = -1;
                    cells[i, j] = new GridCell();
                    //cells[i, j].Adiag = (short)(2 + (i == 0 || i + 1 == max_x ? 1 : 0) + (j == 0 || j + 1 == max_y ? 1 : 0));
                    //cells[i, j].Aplusi = (short)(i + 1 < max_x ? -1 : 0);
                    //cells[i, j].Aplusj = (short)(j + 1 < max_y ? -1 : 0);
                    if (new Vector2(i - max_x / 2, j - max_y / 2).LengthSquared < max_x * max_y / 16)
                        cells[i, j].ImplicitSurface = 5;
                }
            using (TextWriter tw = new StreamWriter("output.txt")) tw.WriteLine(A.ToMatrixString(900, 900));
            solver = new
                MathNet.Numerics.LinearAlgebra.Double.Solvers.Preconditioners.Ilutp();
            solver.Initialize(A);
            p = new DenseVector(max_x * max_y);
            d = new DenseVector(max_x * max_y);
        }

        /// <summary>
        /// Steps:
        /// 1. Advect velocity.
        /// 2. Project for pressure.
        /// 3. Solve for final velocity.
        /// 4. Solve implicit surface.
        /// 5. Advect density.
        /// 6. Advect temperature.
        /// 7. Apply forces.
        /// </summary>
        public void TimeStep()
        {
            //cells[15, 15].Density = 1000;
            //cells[15, 15].Temperature = 2000;

            for (int i = 0; i < max_x; i++)
            {
                for (int j = 0; j < max_y; j++)
                {
                    Vector2d vel = AdvectVelocity(timestep, i, j);
                    u_x[i, j] = vel.X;
                    u_y[i, j] = vel.Y;
                }
            }
            Advect(timestep);
            Project(timestep);
            //error = 0;
            for (int i = 0; i < max_x; i++)
                for (int j = 0; j < max_y; j++)
                {
                    u_x[i, j] += timestep * GlobalForce.X;
                    u_y[i, j] += timestep * GlobalForce.Y;
                    //error = Math.Max(error, Math.Max(u_x[i,j],u_y[i,j]));
                }

            ApplyBuoyancy(timestep);
            //ApplyVorticity(timestep);
            ApplyGravity(timestep);
        }

        //accepts x in [0,1] and y in [0,1]
        public Vector2d Velocity(double x, double y)
        {
            return GetVelocity(x * max_x, y * max_y);
        }

        public void SetVelocity(double x, double y, Vector2d new_vel)
        {
            int i = (int) (max_x * x);
            int j = (int) (max_y * y);

            if (i < 0 || i >= max_x || j < 0 || j >= max_y)
                return;

            u_x[i,j] = new_vel.X;
            u_y[i,j] = new_vel.Y;
        }


        private Vector2d AdvectVelocity(double dt, int i, int j) {
            if (i < 0 || i >= max_x || j < 0 || j >= max_y)
                return new Vector2d(0, 0);
            
            double old_x = i - dt * u_x[i,j];
            double old_y = j - dt * u_y[i,j];
            int old_i = (int)old_x;
            int old_j = (int)old_y;
            double offset_i = old_x - old_i;
            double offset_j = old_y - old_j;

            if (old_i < 0 || old_i >= max_x - 1 || old_j < 0 || old_j >= max_y - 1)
                return new Vector2d(0, 0);

           double ret_x = (offset_i) * (offset_j) * u_x[old_i + 1,old_j + 1]
                 + (offset_i) * (1 - offset_j) * u_x[old_i + 1,old_j]   
                 + (1 - offset_i) * (offset_j) * u_x[old_i,old_j + 1]
                 + (1 - offset_i) * (1 - offset_j) * u_x[old_i,old_j];

            double ret_y = (offset_i) * (offset_j) * u_y[old_i + 1,old_j + 1]
                 + (offset_i) * (1 - offset_j) * u_y[old_i + 1,old_j]
                 + (1 - offset_i) * (offset_j) * u_y[old_i,old_j + 1]
                 + (1 - offset_i) * (1 - offset_j) * u_y[old_i,old_j];

            return new Vector2d(ret_x, ret_y);
        }

        private void Advect(double dt)
        {
            for (int i = 0; i < max_x; i++)
            {
                for (int j = 0; j < max_y; j++)
                {
                    double vel_x = (u_x[i, j] + u_x[i + 1, j]) / 2;
                    double vel_y = (u_y[i, j] + u_y[i, j + 1]) / 2;
                    double old_x = i - dt * vel_x;
                    double old_y = j - dt * vel_y;
                    int old_i = (int)old_x;
                    int old_j = (int)old_y;
                    double offset_i = old_x - old_i;
                    double offset_j = old_y - old_j;
                    // this skips for now but really old_i and old_j should be clamped
                    if (old_i < 0 || old_j < 0 || old_i + 1 >= max_x || old_j + 1 >= max_y) continue;
                    //Advects temperature, density
                    cells[i, j].SetInterpolatedValues(cells[old_i, old_j], cells[old_i, old_j + 1], cells[old_i + 1, old_j], cells[old_i + 1, old_j + 1], offset_i, offset_j);
                    // deal with implicit surfaces
                    if (i <= 0 || j <= 0 || i + 2 >= max_x || j + 2 >= max_y) continue;
                    Vector2d normal = new Vector2d(cells[i + 1, j].ImplicitSurface - cells[i - 1, j].ImplicitSurface, cells[i, j + 1].ImplicitSurface - cells[i, j - 1].ImplicitSurface);
                    normal.Normalize();
                    double w_x = vel_x + FluidConstants.BLUE_CORE_EMISSION_RATE * normal.X;
                    double w_y = vel_y + FluidConstants.BLUE_CORE_EMISSION_RATE * normal.Y;
                    cells[i, j].ImplicitSurface -= timestep * (w_x * (cells[i - 1, j].ImplicitSurface - cells[i, j].ImplicitSurface) + w_y * (cells[i, j - 1].ImplicitSurface - cells[i, j].ImplicitSurface));
                }
            }
        }

        private void ApplyBuoyancy(double dt)
        {
            for (int i = 0; i < max_x; i++)
            {
                for (int j = 1; j < max_y; j++)
                {
                    double f_buoy = ((cells[i, j].Temperature + cells[i, j - 1].Temperature) / 2) * (FluidConstants.BUOYANCY);
                    u_y[i, j] -= f_buoy * dt;
                }
            }

        }

        private void ApplyGravity(double dt)
        {
            for (int i = 0; i < max_x; i++)
            {
                for (int j = 0; j < max_y; j++)
                {
                    u_y[i, j] += FluidConstants.GRAVITY_MAGNITUDE;
                }
            }
        }

        private void ApplyVorticity(double dt)
        {
            for (int i = 1; i < max_x - 1; i++)
            {
                for (int j = 1; j < max_y - 1; j++)
                {
                    //(dv/dx - du/dy)k = curl
                    cells[i,j].Vorticity = (u_x[i, j + 1] - u_x[i, j - 1]) / 2 - (u_y[i + 1, j] - u_y[i - 1, j]) / 2;
                }
            }

            for (int i = 2; i < max_x - 2; i++)
            {
                for (int j = 2; j < max_y - 2; j++)
                {
                    Vector3d curl = new Vector3d(0, 0, cells[i, j].Vorticity);
                    Vector3d N = new Vector3d((Math.Abs(cells[i + 1, j].Vorticity) - Math.Abs(cells[i - 1, j].Vorticity)) / 2,
                                           (Math.Abs(cells[i, j + 1].Vorticity) - Math.Abs(cells[i, j - 1].Vorticity)) / 2,
                                           0);
                    N.Normalize();
                    Vector3d f_vorticity = Vector3d.Cross(N,curl);
                    Vector3d.Mult(f_vorticity, FluidConstants.VORTICITY);

                    u_x[i, j] += (dt * f_vorticity.X) / 2;
                    u_x[i + 1, j] += (dt * f_vorticity.X) / 2;
                    u_y[i, j] += (dt * f_vorticity.Y) / 2;
                    u_y[i, j + 1] += (dt * f_vorticity.Y) / 2;


        }
            }
        }

        private Vector2d GetVelocity(double x, double y)
        {
            if (x <= 0.5 || x >= max_x - 1 || y <= 0.5 || y >= max_y - 1)
                return new Vector2d(0, 0);

            //get x first:
            x += 0.5;

            double velocity_x = 0;
            double offset_x = x - ((int)x);
            double offset_y = y - ((int)y);
            int i = (int)x;
            int j = (int)y;

            velocity_x = (offset_x) * (offset_y) * u_x[i, j]
                + (1 - offset_x) * (offset_y) * u_x[i + 1, j]
                + (offset_x) * (1 - offset_y) * u_x[i, j + 1]
                + (1 - offset_x) * (1 - offset_y) * u_x[i + 1, j + 1];
            x -= 0.5;

            //now y:
            y += 0.5;

            offset_x = x - ((int)x);
            offset_y = y - ((int)y);
            i = (int)x;
            j = (int)y;

            double velocity_y = (offset_x) * (offset_y) * u_y[i, j]
                + (1 - offset_x) * (offset_y) * u_y[i + 1, j]
                + (offset_x) * (1 - offset_y) * u_y[i, j + 1]
                + (1 - offset_x) * (1 - offset_y) * u_y[i + 1, j + 1];

            return new Vector2d(velocity_x, velocity_y);
        }

        private void Project(double timestep)
        {
            d = new DenseVector(max_x * max_y);
            for (int i = 0; i < max_x; i++)
                for (int j = 0; j < max_y; j++)
                    d[i * max_x + j] = u_x[i + 1, j] - u_x[i, j] + u_y[i, j + 1] - u_y[i, j];
            solver.Approximate(d, p);
            for (int i = 0; i < max_x; i++)
                for (int j = 0; j < max_y; j++)
                    cells[i, j].Pressure = p[i * max_x + j];
            error = 0;
            for (int i = 0; i < max_x - 1; i++)
                for (int j = 0; j < max_y - 1; j++)
                {
                    if (cells[i, j].Density == 0) continue;
                    u_x[i + 1, j] -= timestep / cells[i, j].Density * cells[i + 1, j].Pressure - cells[i, j].Pressure;
                    u_y[i, j + 1] -= timestep / cells[i, j].Density * cells[i, j + 1].Pressure - cells[i, j].Pressure;
                    error += Util.Square(u_x[i + 1, j] - u_x[i, j] + u_y[i, j + 1] - u_y[i, j]);
                }
        }

        public void OnClick(double x, double y)
        {
            if (x < 0 || y < 0 || x >= max_x || y >= max_x) return;
            cells[(int)x, (int)y].Temperature += 5000;
            cells[(int)x, (int)y].Density += 10000;
            SpotlightCell = cells[(int)x, (int)y];
        }

        public static GridCell SpotlightCell;
        public double error;

        public void Run()
        {
            _running = true;
            while (_running)
                TimeStep();
        }

        public void Stop()
        {
            _running = false;
        }

        public void Draw()
        {
            GL.PushMatrix();
            for (int i = 0; i < max_x; i++)
            {
                GL.PushMatrix();
                for (int j = 0; j < max_y; j++)
                {
                    cells[i, j].Draw();
                    GL.Translate(0, 1, 0);
                }
                GL.PopMatrix();
                GL.Translate(1, 0, 0);
            }
            GL.PopMatrix();
            GL.Begin(BeginMode.Lines);
            for (int i = 0; i < max_x; i++)
                for (int j = 0; j < max_y; j++)
                {
                    /*GL.Color3(0f,0f,Math.Log10(u_x[i,j])/3);
                    GL.Vertex2(i - 0.5, j);
                    GL.Vertex2(i - 0.5 + u_x[i,j], j);
                    GL.Color3(0f, 0f, Math.Log10(u_y[i, j]) / 3);
                    GL.Vertex2(i, j - 0.5);
                    GL.Vertex2(i, j - 0.5 + u_y[i,j]);*/
                }
            GL.End();
        }

        public double DX
        {
            get
            {
                return timestep;
            }
            set
            {
                timestep = value;
            }
        }
    }
}
