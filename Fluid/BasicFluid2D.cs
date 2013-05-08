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

            u_x[i][j] = (offset_i) * (offset_j) * u_x[old_i + 1][old_j + 1]
                 + (offset_i) * (1 - offset_j) * u_x[old_i + 1][old_j]
                 + (1 - offset_i) * (offset_j) * u_x[old_i][old_j]
                 + (1 - offset_i) * (1 - offset_j) * u_x[old_i][old_j];

            u_y[i][j] = (offset_i) * (offset_j) * u_y[old_i + 1][old_j + 1]
                 + (offset_i) * (1 - offset_j) * u_y[old_i + 1][old_j]
                 + (1 - offset_i) * (offset_j) * u_y[old_i][old_j]
                 + (1 - offset_i) * (1 - offset_j) * u_y[old_i][old_j];
        }

        private void project()
        {
            //matrix solving
        }

    }
}
