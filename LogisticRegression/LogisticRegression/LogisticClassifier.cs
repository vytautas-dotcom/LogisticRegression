using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticRegression
{
    class LogisticClassifier
    {
        private int numFeatures;
        private double[] weights;
        private Random rnd;

        public LogisticClassifier(int numFeatures)
        {
            this.numFeatures = numFeatures;
            this.weights = new double[numFeatures + 1];
        }

        private double[] Centroids(double[] good, double[] best)
        {
            double[] centroids = new double[this.numFeatures + 1];
            for (int i = 0; i < centroids.Length; i++)
            {
                centroids[i] = (good[i] + best[i]) * 0.5;
            }
            return centroids;
        }
        private double[] Expanded(double[] centroids, double[] worst)
        {
            double[] expanded = new double[this.numFeatures + 1];
            for (int i = 0; i < expanded.Length; i++)
            {
                expanded[i] = centroids[i] + ((centroids[i] - worst[i]) * 2); // E=2R-C if reflected is known
            }
            return expanded;
        }
        private double[] Reflected(double[] centroids, double[] worst)
        {
            double[] reflected = new double[this.numFeatures + 1];
            for (int i = 0; i < reflected.Length; i++)
            {
                reflected[i] = 2 * centroids[i] - worst[i]; // R=C-(alpha*(C-W)) if it is needed to change distance 
            }
            return reflected;
        }
        private double[] Contracted(double[] centroids, double[] worst)
        {
            double[] contracted = new double[this.numFeatures + 1];
            for (int i = 0; i < contracted.Length; i++)
            {
                contracted[i] = centroids[i] - (0.5 * (centroids[i] - worst[i])); // - for point inside triangle
            }
            return contracted;
        }
        private double[] RandomPoint()
        {
            rnd = new Random();
            double[] point = new double[this.numFeatures + 1];
            for (int i = 0; i < point.Length; i++)
            {
                point[i] = 20 * rnd.NextDouble() - 10;
            }
            return point;
        }
    }
}
