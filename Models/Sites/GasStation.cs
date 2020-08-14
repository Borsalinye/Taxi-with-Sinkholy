using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace Taxi.Models.Sites
{
    public sealed class GasStation : Address
    {
        public int FuelAmount { get; set; }
        public float FuelPrice { get; set; }
        public void RefuelCar(Automobile auto)
        {
            RefuelStarted += auto.OnRefuelStart;
            RefuelStarted(this, EventArgs.Empty);
            Timer timer = new Timer
            (
                callback: x=>
                {
                    if(auto.Fuel + 5 > auto.MaxFuel && FuelAmount > 5)
                    {
                        auto.Fuel += 5;
                        this.FuelAmount -= 5;

                    }
                    else
                    {
                        RefuelEnded(this, EventArgs.Empty);
                        //Уничтожить таймер
                    }
                },
                state: null,
                dueTime: 10, //Продумать задержку и интервал заправки
                period: 5000
            );
        }
        public EventHandler RefuelStarted = delegate { };
        public EventHandler RefuelEnded = delegate { };

        public GasStation(string street, int fuelAmount, int fuelPrice) : base(street)
        {
            this.FuelAmount = fuelAmount;
            this.FuelPrice = fuelPrice;
        }
    }
}