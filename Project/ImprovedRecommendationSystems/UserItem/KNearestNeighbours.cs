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

        public KNearestNeighbours(double threshold)
        {
            _threshold = threshold;
            _nearestNeighbours = new List<NeighbourObject>();
        }

        public NeighbourObject[] GetNearestNeighbours(Dictionary<int, Dictionary<int, double>> ratings, int individualId, int amountOfNeighbours)
        {
            _individual = ratings[individualId];

            foreach (var user in ratings)
            {
                //Skip the all the actions on the "neighbour" if the neighbour is the individual
                if (user.Key == individualId)
                {
                    continue;
                }

                var similarity = CalculateSimilarity(_individual, user.Value);

                if (similarity > _threshold && HasRatedAdditionalItems(_individual, user.Value) )
                {
                    _nearestNeighbours.Add(new NeighbourObject
                    {
                        Id = user.Key,
                        Ratings = user.Value,
                        Similarity = similarity
                    });
                }
            }

            if (_nearestNeighbours.Count() < amountOfNeighbours)
            {
                return _nearestNeighbours.ToArray();
            }
            return _nearestNeighbours.OrderByDescending(x => x.Similarity).Take(amountOfNeighbours).ToArray();
        }

        private bool HasRatedAdditionalItems(Dictionary<int, double> individual, Dictionary<int, double> neighbour)
        {
            return neighbour.Keys.Count(x => individual.Keys.Any(z => z != x)) > 0;
        }

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