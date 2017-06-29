using System.Collections.Generic;

namespace ImprovedRecommendationSystems.UserItem
{
    public class NeighbourObject
    {
        public int Id { get; set; }
        public Dictionary<int, double> Ratings;
        public double Similarity { get; set; }
    }
}