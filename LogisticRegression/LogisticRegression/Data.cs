using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticRegression
{
    class Data
    {
        public double[][] data = new double[30][];
        public Data()
        {
            data[0] = new double[] { 48, +1, 4.40, 0 };
            data[1] = new double[] { 60, -1, 7.89, 1 };
            data[2] = new double[] { 51, -1, 3.48, 0 };
            data[3] = new double[] { 66, -1, 8.41, 1 };
            data[4] = new double[] { 40, +1, 3.05, 0 };
            data[5] = new double[] { 44, +1, 4.56, 0 };
            data[6] = new double[] { 80, -1, 6.91, 1 };
            data[7] = new double[] { 52, -1, 5.69, 0 };
            data[8] = new double[] { 56, -1, 4.01, 0 };
            data[9] = new double[] { 55, -1, 4.48, 0 };
            data[10] = new double[] { 72, +1, 5.97, 0 };
            data[11] = new double[] { 57, -1, 6.71, 1 };
            data[12] = new double[] { 50, -1, 6.40, 0 };
            data[13] = new double[] { 80, -1, 6.67, 1 };
            data[14] = new double[] { 69, +1, 5.79, 0 };
            data[15] = new double[] { 39, -1, 5.42, 0 };
            data[16] = new double[] { 68, -1, 7.61, 1 };
            data[17] = new double[] { 47, +1, 3.24, 0 };
            data[18] = new double[] { 45, +1, 4.29, 0 };
            data[19] = new double[] { 79, +1, 7.44, 1 };
            data[20] = new double[] { 44, -1, 2.55, 0 };
            data[21] = new double[] { 52, +1, 3.71, 0 };
            data[22] = new double[] { 80, +1, 7.56, 1 };
            data[23] = new double[] { 76, -1, 7.80, 1 };
            data[24] = new double[] { 51, -1, 5.94, 0 };
            data[25] = new double[] { 46, +1, 5.52, 0 };
            data[26] = new double[] { 48, -1, 3.25, 0 };
            data[27] = new double[] { 58, +1, 4.71, 0 };
            data[28] = new double[] { 44, +1, 2.52, 0 };
            data[29] = new double[] { 68, -1, 8.38, 1 };
        }
    }
}
