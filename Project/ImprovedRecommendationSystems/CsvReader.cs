using System;
using System.Collections.Generic;
using System.IO;

namespace ImprovedRecommendationSystems
{
    public class CsvReader
    {
        private static readonly char[] Delimiters = { ',', ';' };

        public static Dictionary<int, Dictionary<int, double>> ReadData()
        {
            try
            {
                using (StreamReader reader = new StreamReader(@"ratingsQuick.csv"))
                {
                    string[] fields;

                    var dataDictornary = new Dictionary<int, Dictionary<int, double>>();

                    while (true)
                    {
                        //Breaks out when at the end
                        string line = reader.ReadLine();
                        if (line == null)
                        {
                            break;
                        }

                        //Splits the rows at the delimiters char
                        fields = line.Split(Delimiters);

                        //If the key does not exist yet in the dictionary, add a new empty nested dictionary and then a value into the new nested dictionary
                        if (!dataDictornary.ContainsKey(Convert.ToInt16(fields[0])))
                        {
                            dataDictornary.Add(Convert.ToInt16(fields[0]), new Dictionary<int, double>());
                        }
                        dataDictornary[Convert.ToInt16(fields[0])].Add(Convert.ToInt32(fields[1]), double.Parse(fields[2].Replace('.', ',')));

                    }
                    Console.WriteLine("Done Reading CSV Data");
                    return dataDictornary;
                }
            }
            //CSV file could not be found
            catch (FileNotFoundException f)
            {
                Console.WriteLine("CSV-File not found \n Exception: {0}", f.Message);
                return null;
            }
            //CSV file could not be accessed
            catch (IOException ioException)
            {
                Console.WriteLine("Cannot open CSV file, please make sure it is not opened in your Excel or any other Editor \n Exception: {0}",
                   ioException);
                return null;
            }
            //Invalid convert cast
            catch (InvalidCastException icException)
            {
                Console.WriteLine("Invalid data input, please input integers and/or doubles \n Exception: {0}",
                    icException);
                return null;
            }
            //Other exceptions
            catch (Exception e)
            {
                Console.WriteLine("Error {0}", e.StackTrace);
                return null;
            }
        }
    }
}