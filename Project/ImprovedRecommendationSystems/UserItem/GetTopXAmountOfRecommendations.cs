using System.Collections.Generic;
using System.Linq;

namespace ImprovedRecommendationSystems.UserItem
{
    public class GetTopXAmountOfRecommendations
    {
        public static KeyValuePair<int, double>[] GetTopXAmountOfRatings(int amountOfRatings, NeighbourObject[] nearestNeighbours, int minAmountOfRatings)
        {
            var ratedByAmountOfNeighbours = new Dictionary<int, int>();

            //Add every found id to the dictionary 
            foreach (var neighbour in nearestNeighbours)
            {
                foreach (var keyValuePair in neighbour.Ratings)
                {
                    if (!ratedByAmountOfNeighbours.ContainsKey(keyValuePair.Key))
                    {
                        ratedByAmountOfNeighbours.Add(keyValuePair.Key, 0);
                    }
                    //+1 everytime a particular key is found
                    ratedByAmountOfNeighbours[keyValuePair.Key]++;
                }
            }

            var bestRatings = new Dictionary<int, double>();
            
            //Calculate the rating for every id rated more than the minAmountOfRatings threshold
            foreach (var movieId in ratedByAmountOfNeighbours.Where(n => n.Value >= minAmountOfRatings))
            {
                var predictedRating = PredictRating.CalculatePredictedRating(movieId.Key, nearestNeighbours);
                bestRatings.Add(movieId.Key, predictedRating);
            }

            return bestRatings.OrderByDescending(x => x.Value).Take(amountOfRatings).ToArray();
        }
    }
}