using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Taxi.Models.Persons;
using Taxi.Models.Persons.Workers;
using Taxi.Models.Sites;

namespace Taxi.Models
{
    public enum TripStates
    {
        New,
        Started,
        Ended
    }
    public enum AdditionalDemand
    {
        WithChild = 250,
        WithAnimal = 200,
    }
    public sealed class Trip : Thing
    {
        public static HashSet<Trip> Trips;
        private static void OnTripInicializate(object sender, EventArgs e)
        {
            Trips.Add((Trip)sender);
        }
        private static void OnTripEnd(object sender, EventArgs e)
        {
            Trips.Remove((Trip)sender);
            sender = null;
        }
        public static float MoneyEarnedPerDay(int tripRate, int TravelTime, AdditionalDemand additionalDemand)
        {
            Trip.Money += ((int)tripRate * TravelTime + (int)additionalDemand);
            return Trip.Money;
        }
        private event EventHandler TripInicializate = delegate { };
        private event EventHandler TripStarted = delegate { };
        private event EventHandler TripEnded = delegate { };
        public Driver Driver { get; private set; }
        public Client Client { get; private set; }
        public Automobile Auto => this.Driver.Automobile;
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public Address StartAdress { get; private set; }
        public Address EndAdress { get; private set; }
        public AdditionalDemand AdditionalDemand { get; private set; }
        public TripStates State { get; private set; }
        public int TravelTime { get; private set; }
        public static float Money { get; set; }
        private void Start()
        {
            this.StartDate = DateTime.Now;
            TripStarted(this, EventArgs.Empty);
        }
        private void End()
        {
            this.EndDate = DateTime.Now;
            MoneyEarnedPerDay(this.Auto.Rate, this.TravelTime, this.AdditionalDemand);
            TripEnded(this, EventArgs.Empty);
        }
        public Trip(Client client, AdditionalDemand additionalDemand, Address startPoint, Address endPoint)
        {
            this.AdditionalDemand = additionalDemand;
            this.Client = client;
            this.StartAdress = startPoint;
            this.EndAdress = endPoint;
            this.State = TripStates.New;
            TripStarted += Driver.OnTripStart;
            TripEnded += Driver.OnTripEnd;
            TripInicializate += Trip.OnTripInicializate;
            TripEnded += Trip.OnTripEnd;
            this.TripInicializate(this, EventArgs.Empty);
        }
        public static bool AddTrip(Trip trip)
        {
            using (Mapping db = new Mapping())
            {
                db.Trips.Add(trip);
                db.SaveChanges();
            }
            return true;
        }
    }
}