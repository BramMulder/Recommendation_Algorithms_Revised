using System;
using System.Collections.Generic;
using System.Linq;

namespace ImprovedRecommendationSystems.UserItem
{
    public static class SimilarityCalculations
    {
        public static double CalculateEculeanDistanceCoefficient(double[] dataX, double[] dataY)
        {
            if (dataX.Length != dataY.Length)
                throw new Exception("Data Array lengths differ. Please make sure all arrays have the same amount of points");

            double distance = 0.0;

            //Calculate euclidean distance
            for (int i = 0; i < dataX.Length; i++)
            {
                double delta = dataX[i] - dataY[i];
                distance += Math.Pow(delta, 2);
            }

            //Calculate coefficient
            var coefficient = 1 / (1 + Math.Sqrt(distance));

            return coefficient;
        }

        public static double CalculateManhattanDistanceCoefficient(double[] dataX, double[] dataY)
        {
            if (dataX.Length != dataY.Length)
                throw new Exception("Data Array lengths differ. Please make sure all arrays have the same amount of points");

            double distance = 0.0;

            //Sum all (absolute) delta values between Xi and Yi
            for (int i = 0; i < dataX.Length; i++)
            {
                double delta = dataX[i] - dataY[i];
                distance = distance + Math.Abs(delta);
            }

            //Calculate coefficient
            var coefficient = 1 / (1 + distance);

            return coefficient;
        }

        public static double CalculatePearsonCoefficient(double[] dataX, double[] dataY)
        {
            if (dataX.Length != dataY.Length)
                throw new Exception("Data Array lengths differ. Please make sure all arrays have the same amount of points");

            // n
            var n = dataX.Length;

            // ∑ x
            var xSum = 0.0;
            // ∑ y     
            var ySum = 0.0;
            // ∑ ( x(i) * y(i) )
            var xySum = 0.0;

            // ∑ x(i)²
            var xSquareSum = 0.0;

            // ∑ y(i)²
            var ySquareSum = 0.0;

            for (int i = 0; i < dataX.Length; i++)
            {
                // ∑ x
                xSum += dataX[i];
                // ∑ y
                ySum += dataY[i];

                // ∑ ( x(i) * y(i) )
                xySum += (dataX[i] * dataY[i]);
                // ∑ x(i)²
                xSquareSum += Math.Pow(dataX[i], 2);
                // ∑ y(i)²
                ySquareSum += Math.Pow(dataY[i], 2);
            }

            //∑ (x(i))²
            var xSumSquared = Math.Pow(xSum, 2);
            //∑ (y(i))²
            var ySumSquared = Math.Pow(ySum, 2);

            double r = (xySum - ((xSum * ySum) / n)) / (Math.Sqrt(xSquareSum - (xSumSquared / n)) * Math.Sqrt(ySquareSum - (ySumSquared / n)));

            return r;
        }


        public static double CalculateCosineSimilarityCoefficient(double[] dataX, double[] dataY)
        {
            if (dataX.Length != dataY.Length)
                throw new Exception("Data Array lengths differ. Please make sure all arrays have the same amount of points");

            // ∑ ( x(i) * y(i) )
            var xySum = 0.0;

            // ||x||
            var dotProductX = 0.0;

            // ||y||
            var dotProductY = 0.0;

            for (int i = 0; i < dataX.Length; i++)
            {
                // ∑ ( x(i) * y(i) )
                xySum += (dataX[i] * dataY[i]);
                // Calculate ||x||
                dotProductX += Math.Pow(dataX[i], 2);
                // Calculate ||y||
                dotProductY += Math.Pow(dataY[i], 2);
            }

            dotProductX = Math.Sqrt(dotProductX);
            dotProductY = Math.Sqrt(dotProductY);

            var similarityCoefficient = xySum / (dotProductX * dotProductY);

            return similarityCoefficient;
        }

        public static double RunCosineSimilarity(Dictionary<int, double> individual, Dictionary<int, double> neighbour)
        {
            //Make sure every item that is rated is added to both users' dictornaries
            foreach (var item in individual)
            {
                if (!neighbour.ContainsKey(item.Key))
                {
                    neighbour.Add(item.Key, 0.0);
                }
            }

            foreach (var item in neighbour)
            {
                if (!individual.ContainsKey(item.Key))
                {
                    individual.Add(item.Key, 0.0);
                }
            }

            var dataX = individual.Values.ToArray();
            var dataY = neighbour.Values.ToArray();

            return CalculateCosineSimilarityCoefficient(dataX, dataY);
        }
    }
}