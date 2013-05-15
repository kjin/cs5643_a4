using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using MathNet.Numerics.LinearAlgebra.Double;

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

        DenseMatrix V;
        DenseMatrix VInv;

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
            for (int i = 0; i < max_x; i++)
                for (int j = 0; j < max_y; j++)
                {
                    cells[i, j] = new GridCell();
                    cells[i, j].Adiag = (short)(2 + (i == 0 || i + 1 == max_x ? 1 : 0) + (j == 0 || j + 1 == max_y ? 1 : 0));
                    cells[i, j].Aplusi = (short)(i + 1 < max_x ? -1 : 0);
                    cells[i, j].Aplusj = (short)(j + 1 < max_y ? -1 : 0);
                    if (new Vector2(i - max_x / 2, j - max_y / 2).LengthSquared < max_x * max_y / 16)
                        cells[i, j].IsFluid = true;
                }

            V = MatrixAlgebra.ToDenseMatrix(new ShallowSubsectionMatrix(new DSTMatrix(2 * max_x + 1), 1, 1, max_x, max_x));
            VInv = MatrixAlgebra.Invert(V);
        }

        public void TimeStep()
        {
            
            for (int i = 0; i < max_x; i++)
            {
                for (int j = 0; j < max_y; j++)
                {
                    Vector2d vel = AdvectVelocity(timestep, i, j);
                    u_x[i, j] = vel.X;
                    u_y[i, j] = vel.Y;

                }
            }
            for (int i = 0; i < max_x; i++)
                for (int j = 0; j < max_y; j++)
                {
                    u_x[i, j] += timestep * GlobalForce.X;
                    u_y[i, j] += timestep * GlobalForce.Y;
                }

            ApplyBuoyancy(timestep);
         
            Advect(timestep);
            Project(timestep);
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
                    double vel_y = (u_y[i,j] + u_y[i,j + 1]) / 2;
                    double old_x = i - dt * vel_x;
                    double old_y = j - dt * vel_y;
                    int old_i = (int)old_x;
                    int old_j = (int)old_y;
                    double offset_i = old_x - old_i;
                    double offset_j = old_y - old_j;
                    // this skips for now but really old_i and old_j should be clamped
                    if (old_i < 0 || old_j < 0 || old_i + 1 >= max_x || old_j + 1 >= max_y) continue;
                    cells[i, j].SetInterpolatedValues(cells[old_i, old_j], cells[old_i + 1, old_j], cells[old_i, old_j + 1], cells[old_i + 1, old_j + 1], offset_i, offset_j);
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
                    u_y[i, j] += f_buoy * dt;
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
                return new Vector2d(0,0);

            //get x first:
            x += 0.5;

            double velocity_x = 0;
            double offset_x = x - ((int)x);
            double offset_y = y - ((int)y);
            int i = (int)x;
            int j = (int)y;

            velocity_x = (offset_x) * (offset_y) * u_x[i,j]
                + (1 - offset_x) * (offset_y) * u_x[i + 1,j]
                + (offset_x) * (1 - offset_y) * u_x[i,j + 1]
                + (1 - offset_x) * (1 - offset_y) * u_x[i + 1,j + 1];
            x -= 0.5;

            //now y:
            y += 0.5;

            offset_x = x - ((int)x);
            offset_y = y - ((int)y);
            i = (int)x;
            j = (int)y;

            double velocity_y = (offset_x) * (offset_y) * u_y[i,j]
                + (1 - offset_x) * (offset_y) * u_y[i + 1,j]
                + (offset_x) * (1 - offset_y) * u_y[i,j + 1]
                + (1 - offset_x) * (1 - offset_y) * u_y[i + 1,j + 1];

            return new Vector2d(velocity_x, velocity_y);
        }

        private void Project(double timestep)
        {
            DenseDelegateMatrix B = new DenseDelegateMatrix(delegate(int i, int j) { return (u_x[i + 1, j] + u_x[i, j]) / 2 + (u_y[i, j + 1] + u_y[i, j] / 2); }, max_x);
            DiagonalDelegateMatrix D = new DiagonalDelegateMatrix(delegate(int ij) { return -4 * Util.Square(Math.Sin(ij * Math.PI / 2 / (max_x + 1))); }, max_x);
            DenseMatrix BHat = MatrixAlgebra.Multiply(VInv, MatrixAlgebra.Multiply(B, V));
            DenseMatrix UHat = new DenseMatrix(B.Rows, B.Columns);
            for (int i = 0; i < UHat.Rows; i++)
                for (int j = 0; j < UHat.Columns; j++)
                    UHat[i, j] = FluidConstants.WATER_PRESSURE / timestep * BHat[i, j] / (D[i, i] + D[j, j]);
            DenseMatrix UInt = MatrixAlgebra.Multiply(V, MatrixAlgebra.Multiply(UHat, VInv));
            GridCellMatrix U = new GridCellMatrix(cells, 3);
            for (int i = 0; i < U.Rows; i++)
                for (int j = 0; j < U.Columns; j++)
                    U[i, j] = UInt[i, j];
        }

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
                    GL.Color3((float)i / max_x, (float)j / max_y, 0);
                    Vector2d velocity = GetVelocity(i, j);
                    GL.Vertex2(i - 0.5, j);
                    GL.Vertex2(i - 0.5 + velocity.X, j);
                    GL.Vertex2(i, j - 0.5);
                    GL.Vertex2(i, j - 0.5 + velocity.Y);
                }
            GL.End();
        }

        //temp
        public void SetPressure(int i, int j, double amount)
        {
            cells[i, j].Pressure = amount;
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
