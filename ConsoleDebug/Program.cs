using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Schema;
using Taxi.Models;
using Taxi.Models.Persons.Workers;
using Taxi.Models.Sites;

namespace ConsoleDebug
{
    class Program
    {
        static void Main(string[] args)
        {
            XDocument xdoc = new XDocument(new XElement("documents", 
                new XElement("passport",
                    new XElement("number", "id"),
                    new XElement("birthDate", "date"),
                    new XElement("issuedDate", "date"),
                    new XElement("gender", "idk"),
                    new XElement("issuedPlace", "place")),
                new XElement("inn", "number"),
                new XElement("driverLicense",
                    new XElement("number", "id"),
                    new XElement("issuedDate", "date"),
                    new XElement("issuedPlace", "place"),
                    new XElement("expirationDate"),
                    new XElement("categories", 
                        new XElement("cat", "a")))));


            Address.CalculateCoordinates();
            Address.InicializateMap();
            int three = 0;
            int five = 0;
            int eight = 0;
            foreach(var kvp in Address.Map)
            {
                if(kvp.Value.Count == 3)
                {
                    three++;
                }
                else if (kvp.Value.Count == 5)
                {
                    five++;
                }
                else
                {
                    eight++;
                }
            }
            Console.WriteLine(Address.TestMap());
            Console.ReadKey();
        }
        static void Main2(string[] args)
        {
            string advice = Console.ReadLine();
            int count = int.Parse(Console.ReadLine());
            List<string> words = new List<string>(capacity: count);
            for(int i = 0; i < count; i++)
            {
                words.Add(Console.ReadLine());
            }
            Console.WriteLine(advice);
            Console.WriteLine(count);
            foreach(string word in words)
            {
                Console.WriteLine(word);
            }
        }
    }
}
