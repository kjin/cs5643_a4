using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluid
{
    public class BasicFluid2D
    {

        //(2 dimensions) with velocity and pressure
        double[][] u_x, u_y, p;
        double max_x, max_y;

        private void advect_velocity(double dt, int i, int j) {
            double old_x = i - dt * u_x[i][j];
            double old_y = j - dt * u_y[i][j];
            int old_i = (int)old_x;
            int old_j = (int)old_y;
            double offset_i = old_x - old_i;
            double offset_j = old_y - old_j;

            double ret_x = (offset_i) * (offset_j) * u_x[old_i + 1][old_j + 1]
                 + (offset_i) * (1 - offset_j) * u_x[old_i + 1][old_j]
                 + (1 - offset_i) * (offset_j) * u_x[old_i][old_j]
                 + (1 - offset_i) * (1 - offset_j) * u_x[old_i][old_j];

            double ret_y = (offset_i) * (offset_j) * u_y[old_i + 1][old_j + 1]
                 + (offset_i) * (1 - offset_j) * u_y[old_i + 1][old_j]
                 + (1 - offset_i) * (offset_j) * u_y[old_i][old_j]
                 + (1 - offset_i) * (1 - offset_j) * u_y[old_i][old_j];

            //return Vector2(ret_x, ret_y)
        }

        private void advect(ref double[][] q)
        {
            for (int i = 0; i < max_x; i++)
            {
                for (int j = 0; j < max_y; j++)
                {
                    double vel_x = (u_x[i][j] + u_x[i+1][j] + u_x[i][j+1] + u_x[i+1][j+1])/4;
                    double vel_y = (u_y[i][j] + u_y[i + 1][j] + u_y[i][j + 1] + u_y[i + 1][j + 1]) / 4;

                    //not finished
                }
            }
        }


        private void get_velocity(double x, double y)
        {
            //if (x <= 0.5 || x >= max_x || y <= 0.5 || y >= max_y)
                //return 0;

            //get x first:
            x += 0.5;

            double velocity_x = 0;
            double offset_x = x - ((int)x);
            double offset_y = y - ((int)y);
            int i = (int)x;
            int j = (int)y;

            velocity_x = (offset_x) * (offset_y) * u_x[i][j]
                + (1 - offset_x) * (offset_y) * u_x[i + 1][j]
                + (offset_x) * (1 - offset_y) * u_x[i][j + 1]
                + (1 - offset_x) * (1 - offset_y) * u_x[i + 1][j + 1];
            x -= 0.5;

            //now y:
            y += 0.5;

            offset_x = x - ((int)x);
            offset_y = y - ((int)y);
            i = (int)x;
            j = (int)y;

            double velocity_y = (offset_x) * (offset_y) * u_y[i][j]
                + (1 - offset_x) * (offset_y) * u_y[i + 1][j]
                + (offset_x) * (1 - offset_y) * u_y[i][j + 1]
                + (1 - offset_x) * (1 - offset_y) * u_y[i + 1][j + 1];

            //return Vector2(velocity_x, velocity_y)
        }

        private void project()
        {
            //matrix solving
        }

    }
}
