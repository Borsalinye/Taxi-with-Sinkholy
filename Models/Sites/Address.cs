using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Taxi.Models.Sites
{
    public class Address : Thing

    {
        private static Random Rand = new Random();
        public static Dictionary<Address, Dictionary<Address, int>> Map = new Dictionary<Address, Dictionary<Address, int>>(capacity: 41);
        public static List<Address> Addresses = new List<Address>();
        public static Dictionary<Address, int[]> AddressesCoordinates = new Dictionary<Address, int[]>();
        public static string[,] MapLayout = new string[5, 5]
        {
            {"SR1", "SR2", "SR3", "SR4", "SR5"},
            {"SR16", "FR1", "FR2", "FR3", "SR6"},
            {"SR15", "FR8", "Depo", "FR4", "SR7"},
            {"SR14", "FR7", "FR6", "FR5", "SR8"},
            {"SR13", "SR12", "SR11", "SR10", "SR9"}
        };
        public string Street { get; private set; }
        public int SpeedLimit { get; private set; }
        private protected Address(string street)
        {
            this.Street = street;
        }
        static Address()
        {
            //TODO: заполнять матрицу города
        }
        public static Address Create(string street)
        {
            return AddressesCoordinates.Keys.Where(x => x.Street == street).SingleOrDefault();
        }
        //public struct ValuePair
        //{
        //    int[] Coordinates;
        //    bool WayToSuburb;
        //}
        public static Tuple<bool, string> CreateMapLayout()
        {
            return Tuple.Create(true, string.Empty);
            //TODO : инициализация матрицы города
        }
        public static void CalculateCoordinates()
        {
            for (int i = 0; i < MapLayout.GetLength(0); i++)
            {
                for(int j = 0; j < MapLayout.GetLength(1); j++)
                {
                    Address adr = new Address(MapLayout[i, j]);
                    int[] coordinates = new int[2] { i, j };
                    Addresses.Add(adr);
                    AddressesCoordinates.Add(adr, coordinates);
                }
            }
        }
        public static List<Tuple<int, int>> GetNeighbors(int a, int b)
        {
            int axisA = MapLayout.GetLength(0) - 1;
            int axisB = MapLayout.GetLength(1) - 1;
            if (a > axisA || a < 0)
            {
                throw new ArgumentOutOfRangeException("a", "Argument is less then 0 or great then matrix.length");
            }
            else if (b > axisB || b < 0)
            {
                throw new ArgumentOutOfRangeException("b", "Argument is less then 0 or great then matrix.length");
            }
            bool aIsFarPoint = a == axisA ? true : false;
            bool aIsNearPoint = a == 0 ? true : false;
            bool bIsFarPoint = b == axisA ? true : false;
            bool bIsNearPoint = b == 0 ? true : false;
            List<Tuple<int, int>> result = new List<Tuple<int, int>>(capacity: 8);
            string direction = string.Empty;
            int directionsCount = 0;
            if (aIsNearPoint)
            {
                directionsCount++;
                direction += "Down";
            }
            else if (aIsFarPoint)
            {
                directionsCount++;
                direction += "Up";
            }

            if (bIsNearPoint)
            {
                directionsCount++;
                direction += "Right";
            }
            else if (bIsFarPoint)
            {
                directionsCount++;
                direction += "Left";
            }

            if (directionsCount == 2)
            {
                if (direction == "UpRight")
                {
                    result.Add(Tuple.Create(a - 1, b));
                    result.Add(Tuple.Create(a, b + 1));
                    result.Add(Tuple.Create(a - 1, b + 1));
                }
                else if (direction == "DownRight")
                {
                    result.Add(Tuple.Create(a, b + 1));
                    result.Add(Tuple.Create(a + 1, b));
                    result.Add(Tuple.Create(a + 1, b + 1));
                }
                else if (direction == "UpLeft")
                {
                    result.Add(Tuple.Create(a - 1, b));
                    result.Add(Tuple.Create(a, b - 1));
                    result.Add(Tuple.Create(a - 1, b - 1));
                }
                else
                {
                    result.Add(Tuple.Create(a + 1, b));
                    result.Add(Tuple.Create(a, b - 1));
                    result.Add(Tuple.Create(a + 1, b - 1));
                }
            }
            else if (directionsCount == 1)
            {
                if (direction == "Right")
                {
                    result.Add(Tuple.Create(a, b + 1));
                    result.Add(Tuple.Create(a - 1, b));
                    result.Add(Tuple.Create(a + 1, b));
                    result.Add(Tuple.Create(a - 1, b + 1));
                    result.Add(Tuple.Create(a + 1, b + 1));
                }
                else if (direction == "Left")
                {
                    result.Add(Tuple.Create(a, b - 1));
                    result.Add(Tuple.Create(a - 1, b));
                    result.Add(Tuple.Create(a + 1, b));
                    result.Add(Tuple.Create(a - 1, b - 1));
                    result.Add(Tuple.Create(a + 1, b - 1));
                }
                else if (direction == "Up")
                {
                    result.Add(Tuple.Create(a, b + 1));
                    result.Add(Tuple.Create(a, b - 1));
                    result.Add(Tuple.Create(a - 1, b));
                    result.Add(Tuple.Create(a - 1, b + 1));
                    result.Add(Tuple.Create(a - 1, b - 1));
                }
                else
                {
                    result.Add(Tuple.Create(a, b + 1));
                    result.Add(Tuple.Create(a, b - 1));
                    result.Add(Tuple.Create(a + 1, b));
                    result.Add(Tuple.Create(a + 1, b - 1));
                    result.Add(Tuple.Create(a + 1, b + 1));
                }
            }
            else
            {
                result.Add(Tuple.Create(a + 1, b));
                result.Add(Tuple.Create(a - 1, b));
                result.Add(Tuple.Create(a, b + 1));
                result.Add(Tuple.Create(a, b - 1));
                result.Add(Tuple.Create(a + 1, b + 1));
                result.Add(Tuple.Create(a - 1, b + 1));
                result.Add(Tuple.Create(a + 1, b - 1));
                result.Add(Tuple.Create(a - 1, b - 1));
            }
            return result;
        }
        public static void InicializateMap()
        {
            foreach (var address in AddressesCoordinates)
            {
                Address currentAddress = address.Key;
                Map.Add(currentAddress, new Dictionary<Address, int>());
                List<Tuple<int, int>> neighbors = GetNeighbors(address.Value[0], address.Value[1]);
                foreach(var neighborCoordinates in neighbors)
                {
                    string street = MapLayout[neighborCoordinates.Item1, neighborCoordinates.Item2];
                    Address neighbor = Addresses.Where(x => x.Street == street).SingleOrDefault();
                    if (Map.ContainsKey(neighbor))
                    {
                        if (Map[neighbor].ContainsKey(currentAddress))
                        {
                            Map[currentAddress].Add(neighbor, Map[neighbor][currentAddress]);
                        }
                        else
                        {
                            Map[currentAddress].Add(neighbor, Rand.Next(1, 10));
                        }
                    }
                    else
                    {
                        Map[currentAddress].Add(neighbor, Rand.Next(1, 10));
                    }
                }
            }
        }
        public static bool TestMap()
        {
            foreach(Address address in Addresses)
            {
                List<Address> neighbors = Map[address].Keys.ToList();
                foreach(Address neighbor in neighbors)
                {
                    if (Map[address][neighbor] != Map[neighbor][address])
                    {
                        return false;
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            return true;
        }
    }
}