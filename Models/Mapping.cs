using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Taxi.Models.Persons;
using Taxi.Models.Persons.Workers;

namespace Taxi.Models
{
    public class Mapping : DbContext
    {
        public Mapping() : base("TaxiDB")
        { }
        public DbSet<Person> People { get; set; }
        public DbSet<Automobile> Automobiles { get; set; }
        public DbSet<Trip> Trips { get; set; }
    }
}