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
        //POINTS
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

        //Sigmoid calculation for every entity
        public double Output(double[] dataItem, double[] weights)
        {
            double z = 0.0;
            z += weights[0];
            for (int i = 1; i < dataItem.Length - 1; i++)
            {
                z += weights[i] * dataItem[i - 1];
            }
            double sigmoid = 1.0 / (1.0 + Math.Exp(-z));
            return sigmoid;
        }
        public int Dependent(double[] dataItem, double[] weights)
        {
            double sum = Output(dataItem, weights);
            if (sum <= 0.5)
                return 0;
            else
                return 1;
        }
        public double Accuracy(double[][] trainData, double[] weights)
        {
            int correct = 0;
            int wrong = 0;
            int yColumn = trainData[0].Length - 1;
            for (int i = 0; i < trainData.Length; i++)
            {
                double computed = Dependent(trainData[i], weights);
                double desired = trainData[i][yColumn];
                if (computed == desired)
                    correct++;
                else
                    wrong++;
            }
            double accuracy = (double)correct / (correct + wrong);
            return accuracy;
        }
        private double Error(double[][] trainData, double[] weights)
        {
            int yColumn = trainData[0].Length - 1;
            double sumSquaredError = 0.0;
            for (int i = 0; i < trainData.Length; i++)
            {
                double computed = Output(trainData[i], weights);
                double desired = trainData[i][yColumn];
                sumSquaredError += (computed - desired) * (computed - desired);
            }
            return sumSquaredError / trainData.Length;
        }
    }
}
