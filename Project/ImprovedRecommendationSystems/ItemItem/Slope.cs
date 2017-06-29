using System.Collections.Generic;
using System.Linq;

namespace ImprovedRecommendationSystems.ItemItem
{
    class Slope
    {
        public static DeviationObject GenerateDeviationObject(Dictionary<int, Dictionary<int, double>> ratings, int itemId1, int itemId2)
        {
            var userKeys = ratings.Where(x => x.Value.ContainsKey(itemId1) && x.Value.ContainsKey(itemId2)).Select(x => x.Key).ToArray();

            if (!userKeys.Any())
                return new DeviationObject();

            return new DeviationObject()
            {
                AmountOfRatings = userKeys.Length,
                Deviation = CalculateDeviation(ratings, userKeys, itemId1, itemId2)
            };
        }

        private static double CalculateDeviation(Dictionary<int, Dictionary<int, double>> userRatings, int[] userKeys, int itemId1, int itemId2)
        {
            var currDev = userKeys.Sum(key => userRatings[key][itemId1] - userRatings[key][itemId2]);

            return currDev / userKeys.Length;
        }
    }
}