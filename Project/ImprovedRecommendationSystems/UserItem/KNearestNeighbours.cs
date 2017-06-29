using System;
using System.Collections.Generic;
using System.Linq;

namespace ImprovedRecommendationSystems.UserItem
{
    public class KNearestNeighbours
    {
        private List<NeighbourObject> _nearestNeighbours;
        private double _threshold;
        private Dictionary<int, double> _individual;

        //Initialize
        public KNearestNeighbours(double threshold)
        {
            _threshold = threshold;
            _nearestNeighbours = new List<NeighbourObject>();
        }

        public NeighbourObject[] GetNearestNeighbours(Dictionary<int, Dictionary<int, double>> ratings, int individualId, int amountOfNeighbours)
        {
            //Get ratings from the tagret user
            _individual = ratings[individualId];

            //Loop all users which have rated items
            foreach (var user in ratings)
            {
                //Skip the all the actions on the "neighbour" if the neighbour is the individual
                if (user.Key == individualId)
                {
                    continue;
                }

                //Calculate the similarity between the target user and a neighbour, "user"
                var similarity = CalculateSimilarity(_individual, user.Value);

                //If the similarity is larger or equal to the threshold and has rated an additional item to the target user, add it to the nearest neighbour list
                if (similarity >= _threshold && HasRatedAdditionalItems(_individual, user.Value) )
                {
                    _nearestNeighbours.Add(new NeighbourObject
                    {
                        Id = user.Key,
                        Ratings = user.Value,
                        Similarity = similarity
                    });
                }
            }

            //If there are less neighbours in the list than requested, return the entire list
            if (_nearestNeighbours.Count() < amountOfNeighbours)
            {
                return _nearestNeighbours.ToArray();
            }
            //Else take the top X
            return _nearestNeighbours.OrderByDescending(x => x.Similarity).Take(amountOfNeighbours).ToArray();
        }

        //Checks if the neighbour has rated an additional item to the target user
        private bool HasRatedAdditionalItems(Dictionary<int, double> individual, Dictionary<int, double> neighbour)
        {
            return neighbour.Keys.Count(x => individual.Keys.Any(z => z != x)) > 0;
        }

        //Calculates the similarity between two given users
        private double CalculateSimilarity(Dictionary<int, double> individual, Dictionary<int, double> neighbour)
        {
            //Select the items that both users have rated
            var ratingsIndividual = (from i in individual
                                     join n in neighbour
                                     on i.Key equals n.Key
                                     select Convert.ToDouble(i.Value)).ToArray();
            var ratingsNeighbour = (from i in individual
                                    join n in neighbour
                                    on i.Key equals n.Key
                                    select Convert.ToDouble(n.Value)).ToArray();

            var pCoefficient = SimilarityCalculations.CalculatePearsonCoefficient(ratingsIndividual, ratingsNeighbour);

            return pCoefficient;
        }
    }
}