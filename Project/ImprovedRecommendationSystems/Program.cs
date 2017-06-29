using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImprovedRecommendationSystems.ItemItem;
using ImprovedRecommendationSystems.UserItem;

namespace ImprovedRecommendationSystems
{
    class Program
    {
        static void Main(string[] args)
        {
            var data = CsvReader.ReadData();

            var userId = 186;
            var itemId = 1;

            //User-Item
            KNearestNeighbours kNearestNeighbours = new KNearestNeighbours(0.35);
            var nearestNeighbours = kNearestNeighbours.GetNearestNeighbours(data, userId, 25);
            var topRatings = GetTopXAmountOfRecommendations.GetTopXAmountOfRatings(8, nearestNeighbours, 3);

            //Item-Item
            var top5 = ItemItemLogic.RunItemItemMethods(data, userId);
        }
    }
}
