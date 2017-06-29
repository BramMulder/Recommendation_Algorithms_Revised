using System;
using System.Collections.Generic;
using System.Linq;

namespace ImprovedRecommendationSystems.ItemItem
{
    public class ItemItemLogic
    {
        public static List<Tuple<double, double>> RunItemItemMethods(Dictionary<int, Dictionary<int, double>> dictionary, int targetUserId)
        {
            //Select all Item Ids per User
            var movieIds = dictionary.Values.Select(x => x.Keys).ToArray();
            //Get all unique Ids in a sorted list
            var uniqueIdsOrdered = GetAllUniqueIds(movieIds);


            var deviationsDictionary = GenerateDeviationsDictionary(dictionary, uniqueIdsOrdered);
            
            //Retrieve top 5 ratings for given user
            var top5 = FindTopKSuggestions(dictionary, deviationsDictionary, 5, targetUserId);
            return top5;
        }

        private static int[] GetAllUniqueIds(Dictionary<int, double>.KeyCollection[] movieIds)
        {
            var uniqueItemIds = new List<int>();
            //Create a list of unique Item Ids
            for (int j = 0; j < movieIds.Length; j++)
            {
                var id = movieIds[j].ToArray();
                for (int i = 0; i < id.Length; i++)
                {
                    if (!uniqueItemIds.Contains(id[i]))
                    {
                        uniqueItemIds.Add(id[i]);
                    }
                }
            }
            //Order Ids Asscendingly
            var uniqueItemsOrdered = uniqueItemIds.OrderBy(x => x).ToArray();

            return uniqueItemsOrdered;
        }

        private static Dictionary<int, Dictionary<int, DeviationObject>> GenerateDeviationsDictionary(Dictionary<int, Dictionary<int, double>> data, int[] uniqueIds)
        {
            var deviations = new Dictionary<int, Dictionary<int, DeviationObject>>();

            //Loop over half the data and calulate the deviations
            for (int i = 0; i < uniqueIds.Length; i++)
            {
                //Add a new row
                deviations.Add(uniqueIds[i], new Dictionary<int, DeviationObject>());
                for (int j = i; j < uniqueIds.Length; j++)
                {
                    //Skip comparing the item to itself
                    if (i == j)
                    {
                        deviations[uniqueIds[i]].Add(uniqueIds[j], new DeviationObject());
                        continue;
                    }
                    //Add the deviation values at the correct index
                    deviations[uniqueIds[i]].Add(uniqueIds[j], Slope.GenerateDeviationObject(data, uniqueIds[i], uniqueIds[j]));
                }
            }

            //Loop over the other half and inverse the deviation values
            for (int a = 1; a < uniqueIds.Length; a++)
            {
                for (int b = 0; b < a; b++)
                {
                    if (a == b)
                    {
                        continue;
                    }
                    var deviationObject = deviations[uniqueIds[b]][uniqueIds[a]];
                    var newDeviationObject = new DeviationObject()
                    {
                        AmountOfRatings = deviationObject.AmountOfRatings,
                        Deviation = deviationObject.Deviation * -1
                    };
                    deviations[uniqueIds[a]].Add(uniqueIds[b], newDeviationObject);
                }
            }

            return deviations;
        }

        private static List<Tuple<double, double>> FindTopKSuggestions(Dictionary<int, Dictionary<int, double>> data, Dictionary<int, Dictionary<int, DeviationObject>> deviationsDictionary, int k, int targetUserId)
        {
            //Select all Ids
            var movieIds = deviationsDictionary.Keys.ToList();
            //Select the data from the target user
            var userData = data[targetUserId];
            List<Tuple<double, double>> topRatings = new List<Tuple<double, double>>();

            //Loop al items
            foreach (var movieId in movieIds)
            {
                //If the user hasn't rated the item yet, add it to the list
                if (!userData.ContainsKey(movieId))
                {
                    double prediction = PredictedRating.PredictRating(data, deviationsDictionary, targetUserId, movieId);
                    topRatings.Add(new Tuple<double, double>(movieId, prediction));
                }
            }
            return topRatings.OrderByDescending(t => t.Item2).Take(k).ToList();
        }

        private static Dictionary<double, Dictionary<double, DeviationObject>> AddRatingUpdateDeviations(Dictionary<int, Dictionary<int, double>> data, Dictionary<double, Dictionary<double, DeviationObject>> deviationsDictionary, int userId)
        {
            var user = data[userId];

            var userRatedList = user.Keys.ToList();

            //Update deviations after a new rating has been added by a user
            foreach (var ratingId in userRatedList)
            {
                var deviation = Slope.GenerateDeviationObject(data, userId, ratingId);
                deviationsDictionary[userId][ratingId] = Slope.GenerateDeviationObject(data, userId, ratingId);
                deviationsDictionary[ratingId][userId] = new DeviationObject { AmountOfRatings = deviation.AmountOfRatings, Deviation = deviation.Deviation * -1};
            }
            return deviationsDictionary;
        }
    }
}