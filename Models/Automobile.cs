using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Taxi.Models.Persons.Workers;

namespace Taxi.Models
{
    public enum AutomobileClassification : byte
    {
        Economy,
        Standart,
        Luxury
    }
    public enum TripRate : byte
    {
        EconomRate = 5,
        StandartRate = 10,
        LuxuryRate = 15
    }
    public enum FuelTank : byte
    {
        EconomyTank = 50,
        StandartTank = 70,
        LuxuryTank = 100
    }
    public enum MaxAutoSpeed : int
    {
        EconomySpeed = 180,
        StandartSpeed = 230,
        LuxurySpeed = 290
    }
    public sealed class Automobile : Thing
    {
        public int MaxSpeed { get; private set; }
        public bool Radio { get; private set; }
        public bool Internet { get; private set; }
        public bool Conditioner { get; private set; }
        public Driver Driver { get; set; }
        public string GovermentId { get; private set; }
        public int MaxFuel { get; set; }
        public int Rate { get; set; }
        public int Fuel { get; set; }
        public bool ParkingAuto { get; private set; }
        public int RestFuelAmount => (Fuel/MaxFuel)*100 ;
        public void OnRefuelStart(object sender, EventArgs e) { }
        public void OnRefuelEnd(object sender, EventArgs e) { }
        public AutomobileClassification Classification
        {
            get => Classification;
            private set
            {
                Classification = value;
                switch (value)
                {
                    case AutomobileClassification.Economy:
                        this.MaxSpeed = (int)MaxAutoSpeed.EconomySpeed;
                        this.MaxFuel = (int)FuelTank.EconomyTank;
                        this.Rate = (int)TripRate.EconomRate;
                        Radio = true;
                        Internet = false;
                        Conditioner = false;
                        break;
                    case AutomobileClassification.Standart:
                        this.MaxSpeed = (int)MaxAutoSpeed.StandartSpeed;
                        this.MaxFuel = (int)FuelTank.StandartTank;
                        this.Rate = (int)TripRate.StandartRate;
                        Radio = true;
                        Internet = false;
                        Conditioner = true;
                        break;
                    case AutomobileClassification.Luxury:
                        this.MaxSpeed = (int)MaxAutoSpeed.LuxurySpeed;
                        this.MaxFuel = (int)FuelTank.LuxuryTank;
                        this.Rate = (int)TripRate.LuxuryRate;
                        Radio = true;
                        Internet = true;
                        Conditioner = true;
                        break;
                    default:
                        break;
                }
            }
        }
        public Automobile(AutomobileClassification classification, string govermentNumber, bool parkingAuto)
        {
            this.GovermentId = govermentNumber;
            Classification = classification;
            this.ParkingAuto = parkingAuto;
        }
        public static bool AddAuto(Automobile automobile)
        {
            using (Mapping db = new Mapping())
            {
                db.Automobiles.Add(automobile);
                db.SaveChanges();
            }
            return true;
        }
    }
}