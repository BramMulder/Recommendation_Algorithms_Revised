using System;
using System.Collections.Generic;
using System.Linq;

namespace ImprovedRecommendationSystems.ItemItem
{
    public class PredictedRating
    {
        public static double PredictRating(Dictionary<int, Dictionary<int, double>> usersRatings, Dictionary<int, Dictionary<int, DeviationObject>> deviationsDictionary , int targetUser, int targetItem)
        {
            var userRatings = usersRatings[targetUser].Values.ToArray();
            var itemDeviations = (from i in usersRatings[targetUser]
                join n in deviationsDictionary[targetItem]
                on i.Key equals n.Key
                select n.Value).ToArray();

            if(userRatings.Length != itemDeviations.Length)
                Console.WriteLine("NOT EQUAL!");

            double numerator = 0;
            double denominator = 0;

            for (int i = 0; i < userRatings.Length; i++)
            {
                var userRating = userRatings[i];
                var deviation = itemDeviations[i];

                numerator += (userRating + deviation.Deviation) * deviation.AmountOfRatings;
                denominator += deviation.AmountOfRatings;
            }

            return numerator / denominator;
        }
    }
}