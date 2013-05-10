using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Fluid
{
    public class BasicFluid2D : GLDrawable
    {

        //(2 dimensions) with velocity and pressure
        double[,] u_x, u_y, p;
        int max_x, max_y;

        public BasicFluid2D(int max_x, int max_y)
        {
            this.max_x = max_x;
            this.max_y = max_y;

            u_x = new double[max_x+1,max_y];
            u_y = new double[max_x,max_y+1];
            p = new double[max_x,max_y];
        }

        public void TimeStep(double timestep)
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

            if (old_i < 0 || old_i >= max_x || old_j < 0 || old_j >= max_y)
                return new Vector2d(0, 0);

           double ret_x = (offset_i) * (offset_j) * u_x[old_i + 1,old_j + 1]
                 + (offset_i) * (1 - offset_j) * u_x[old_i + 1,old_j]
                 + (1 - offset_i) * (offset_j) * u_x[old_i,old_j]
                 + (1 - offset_i) * (1 - offset_j) * u_x[old_i,old_j];

            double ret_y = (offset_i) * (offset_j) * u_y[old_i + 1,old_j + 1]
                 + (offset_i) * (1 - offset_j) * u_y[old_i + 1,old_j]
                 + (1 - offset_i) * (offset_j) * u_y[old_i,old_j]
                 + (1 - offset_i) * (1 - offset_j) * u_y[old_i,old_j];

            return new Vector2d(ret_x, ret_y);
        }

        private void Advect(ref double[][] q)
        {
            for (int i = 0; i < max_x; i++)
            {
                for (int j = 0; j < max_y; j++)
                {
                    double vel_x = (u_x[i,j] + u_x[i+1,j])/2;
                    double vel_y = (u_y[i,j] + u_y[i,j + 1]) / 2;

                    //not finished
                }
            }
        }


        private Vector2d GetVelocity(double x, double y)
        {
            if (x <= 0.5 || x >= max_x || y <= 0.5 || y >= max_y)
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

        private void project()
        {
            //matrix solving
        }

        public void Draw()
        {
            GL.Begin(BeginMode.Lines);
            for (int i = 0; i < max_x; i++)
                for (int j = 0; j < max_y; j++)
                {
                    GL.Vertex2(i, j);
                    GL.Vertex2(i + u_x[i,j], j + u_y[i,j]);
                }
            GL.End();
        }
    }
}
