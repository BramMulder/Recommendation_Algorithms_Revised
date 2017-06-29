using System.Collections.Generic;
using System.Linq;

namespace ImprovedRecommendationSystems.UserItem
{
    public class PredictRating
    {
        public static double CalculatePredictedRating(int itemId, NeighbourObject[] nearestNeighbours)
        {
            //Discard all neighbours which havent rated the target item
            var filteredNearestNeighbours = nearestNeighbours.Where(x => x.Ratings.ContainsKey(itemId));

            var ratingSimilaritySum = 0.0;
            var similaritySum = 0.0;

            //Calculate the predicted rating
            foreach (var neighbourRating in filteredNearestNeighbours)
            {
                ratingSimilaritySum += neighbourRating.Similarity * neighbourRating.Ratings[itemId];
                similaritySum += neighbourRating.Similarity;
            }

            var predRating = ratingSimilaritySum / similaritySum;

            return predRating;
        }
    }
}