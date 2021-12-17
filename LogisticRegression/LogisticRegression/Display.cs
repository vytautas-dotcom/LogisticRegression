using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticRegression
{
    class Display
    {
        public void Show()
        {
            Data data = new Data();
            double[][] rawData = data.data;
            Console.WriteLine("\nRaw data: \n");
            Console.WriteLine("      Age    Sex  Kidney Died");
            Console.WriteLine("=======================================");
            ShowData(rawData, 5, 2, true);

            Console.WriteLine("Normalizing age and kidney data");
            int[] columns = new int[] { 0, 2 };
            Normalize(rawData, columns);
            Console.WriteLine("Done");
            Console.WriteLine("\nNormalized data: \n");
            Console.WriteLine("      Age    Sex  Kidney Died");
            Console.WriteLine("=======================================");
            ShowData(rawData, 5, 2, true);

            Console.WriteLine($"Split data: 80% for training, 20% for testing");
            SplitTrainTestData(rawData, 80, out double[][] train, out double[][] test);

            int numOfFeatures = 3;

            LogisticClassifier logisticClassifier = new LogisticClassifier(numOfFeatures);
            int maxEpochs = 100;

            Console.WriteLine($"Training using simplex optimization through {maxEpochs} epochs");

            double[] bestWeights = logisticClassifier.Train(train, maxEpochs);

            Console.WriteLine("Best weights found:");
            ShowVector(bestWeights, 4, true);

            double trainAccuracy = logisticClassifier.Accuracy(train, bestWeights);
            Console.WriteLine($"Prediction accuracy on training data = {trainAccuracy.ToString("F4")}");

            double testAccuracy = logisticClassifier.Accuracy(test, bestWeights);
            Console.WriteLine($"Prediction accuracy on testing data = {testAccuracy.ToString("F4")}");

        }
        static void ShowVector(double[] vector, int decimals, bool newLine)
        {
            for (int i = 0; i < vector.Length; ++i)
                Console.Write(vector[i].ToString("F" + decimals) + " ");
            Console.WriteLine("");
            if (newLine == true)
                Console.WriteLine("");
        }
        static void ShowData(double[][] data, int numRows, int decimals, bool indices)
        {
            for (int i = 0; i < numRows; ++i)
            {
                if (indices == true)
                    Console.Write("[" + i.ToString().PadLeft(2) + "] ");
                for (int j = 0; j < data[i].Length; ++j)
                {
                    double v = data[i][j];
                    if (v >= 0.0)
                        Console.Write(" "); // '+'
                    Console.Write(v.ToString("F" + decimals) + " ");
                }
                Console.WriteLine("");
            }
            Console.WriteLine(". . .");
            int lastRow = data.Length - 1;
            if (indices == true)
                Console.Write("[" + lastRow.ToString().PadLeft(2) + "] ");
            for (int j = 0; j < data[lastRow].Length; ++j)
            {
                double v = data[lastRow][j];
                if (v >= 0.0)
                    Console.Write(" "); // '+'
                Console.Write(v.ToString("F" + decimals) + " ");
            }
            Console.WriteLine("\n");

        }

        static double[][] Normalize(double[][] rawData, int[] columns)
        {
            int rows = rawData.Length;
            int cols = rawData[0].Length;

            double[][] Mean_STD = new double[2][];
            double[][] normalizedData = new double[rawData.Length][];

            for (int i = 0; i < 2; i++)
            {
                Mean_STD[i] = new double[cols];
            }
            //mean and standard deviation for each column
            for (int i = 0; i < cols; i++)
            {
                double sum = 0.0;
                for (int j = 0; j < rows; j++)
                {
                    sum += rawData[j][i];
                }
                double mean = sum / rows;
                Mean_STD[0][i] = mean;

                double sumSquares = 0.0;
                for (int k = 0; k < rows; k++)
                {
                    sumSquares += (rawData[k][i] - mean) * (rawData[k][i] - mean);
                }
                double stdDev = Math.Sqrt(sumSquares / rows);
                Mean_STD[1][i] = stdDev;
            }
            //normalize
            for (int i = 0; i < columns.Length; i++)
            {
                int column = columns[i];
                double mean = Mean_STD[0][column];
                double stdDev = Mean_STD[1][column];
                for (int j = 0; j < rows; j++)
                {
                    rawData[j][column] = (rawData[j][column] - mean) / stdDev;
                }
            }
            return Mean_STD;
        }
        public void SplitTrainTestData(double[][] rawData, double percentForTrain, out double[][] train, out double[][] test)
        {
            train = rawData.Take((int)Math.Floor(rawData.Length * (percentForTrain / 100))).ToArray();
            test = rawData.Skip((int)Math.Floor(rawData.Length * (percentForTrain / 100))).ToArray();

            Console.WriteLine($"Creating train ({percentForTrain}%) and test ({100 - percentForTrain}%) matrices");
        }
    }
}
