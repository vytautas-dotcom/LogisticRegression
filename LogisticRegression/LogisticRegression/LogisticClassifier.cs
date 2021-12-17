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

        private class Solution : IComparable<Solution>
        {
            public double[] weights;
            public double error;
            public Solution(int numFeatures)
            {
                this.weights = new double[numFeatures + 1];
                this.error = 0.0;
            }
            public int CompareTo(Solution other)
            {
                if (this.error < other.error)
                    return -1;
                else if (this.error > other.error)
                    return 1;
                else
                    return 0;
            }
        }

        public double[] Train(double[][] trainData, int maxEpochs)
        {
            rnd = new Random();

            Solution[] solutions = new Solution[3];
            for (int i = 0; i < 3; i++)
            {
                solutions[i] = new Solution(numFeatures);
                solutions[i].weights = RandomPoint();
                solutions[i].error = Error(trainData, solutions[i].weights);
            }
            int best = 0;
            int good = 1;
            int worst = 2;

            int epoch = 0;

            while(epoch < maxEpochs)
            {
                ++epoch;
                Array.Sort(solutions);
                double[] bestWeights = solutions[best].weights;
                double[] goodWeights = solutions[good].weights;
                double[] worstWeights = solutions[worst].weights;

                double[] centroidWeights = Centroids(goodWeights, bestWeights);

                double[] expandedWeights = Expanded(centroidWeights, worstWeights);
                double expandedError = Error(trainData, expandedWeights);

                if (expandedError < solutions[worst].error)
                {
                    Array.Copy(expandedWeights, worstWeights, numFeatures + 1);
                    solutions[worst].error = expandedError;
                    continue;
                }

                double[] reflectedWeights = Reflected(centroidWeights, worstWeights);
                double reflectedError = Error(trainData, reflectedWeights);

                if (reflectedError < solutions[worst].error)
                {
                    Array.Copy(reflectedWeights, worstWeights, numFeatures + 1);
                    solutions[worst].error = reflectedError;
                    continue;
                }

                double[] contractedWeights = Contracted(centroidWeights, worstWeights);
                double contractedError = Error(trainData, contractedWeights);

                if (contractedError < solutions[worst].error)
                {
                    Array.Copy(contractedWeights, worstWeights, numFeatures + 1);
                    solutions[worst].error = contractedError;
                    continue;
                }

                double[] randomPoint = RandomPoint();
                double randomPointError = Error(trainData, randomPoint);

                if (randomPointError < solutions[worst].error)
                {
                    Array.Copy(randomPoint, worstWeights, numFeatures + 1);
                    solutions[worst].error = randomPointError;
                    continue;
                }

                //SHRINK
                //WORST TO BEST/GOOD TO BEST
                for (int i = 0; i < numFeatures + 1; i++)
                {
                    worstWeights[i] = (worstWeights[i] + bestWeights[i]) * 0.5;
                    goodWeights[i] = (goodWeights[i] + bestWeights[i]) * 0.5;
                }
                solutions[worst].error = Error(trainData, worstWeights);
                solutions[good].error = Error(trainData, goodWeights);
            }
            Array.Copy(solutions[best].weights, this.weights, this.numFeatures + 1);
            return this.weights;
        }
    }
}
