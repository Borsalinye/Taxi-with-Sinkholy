using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Taxi.Models.Sites;

namespace Taxi.Models.Persons.Workers
{
    public sealed class Driver : Worker
    {
        public Automobile Automobile {  get; private set; }
        public Address Address { get; private set; }
        public float Salary { get; private set; }
        private bool CheckFuel => this.Automobile.RestFuelAmount >= 20 ? false : true;
        private EventHandler EndDay = delegate { };
        private void DayEnd()
        {
            EndDay(this, EventArgs.Empty);
            CalculateSalary(this.State, this.Automobile.ParkingAuto);
        }
        private GasStation FindClosestGasStation()
        {
            GasStation gasStation = new GasStation(Address.Map.FirstOrDefault(x => x.Key == this.Address).Value.Where(y => y.Value == 1).Select(z => z.Key.Street).FirstOrDefault(), 12, 12);
            return gasStation;
        }
        public override void OnTripEnd(object sender, EventArgs e)
        {
            this.Address = (sender as Trip).EndAdress;
            if (CheckFuel == true)
            {
                this.State = WorkerStates.Available;
            }
            else
            {
                FindClosestGasStation().RefuelCar(this.Automobile);
            }
        }
        private float CalculateSalary(WorkerStates workerStates, bool ParkingCar)
        {
            if (workerStates != WorkerStates.Absent)
            {
                switch(ParkingCar)
                {
                    case true:
                        Salary = Trip.Money * 0.25f;
                        break;
                    case false:
                        Salary = Trip.Money * 0.5f;
                        break;
                }
            }
            else
            {
                Salary = 0;
            }
            return Salary;
        }
    }
}