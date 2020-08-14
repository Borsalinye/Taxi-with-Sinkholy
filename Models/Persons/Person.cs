using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Taxi.Models.Persons.Workers;

namespace Taxi.Models.Persons
{
    public enum Gender : byte
    {
        Male,
        Female,
        Helicopter
    }
    public abstract class Person : Thing
    {
        public static bool AddPerson<TPerson>(TPerson Person)
            where TPerson : Person
        {
            using (Mapping db = new Mapping())
            {
                db.People.Add(Person);
                db.SaveChanges();
            }
            return true;
        }
        public static bool RemovePerson<TPerson>(TPerson Person)
            where TPerson : Person
        {
            using (Mapping db = new Mapping())
            {
                db.People.Remove(Person);
                db.SaveChanges();
            }
            return true;
        }
        public string FullName() => string.Join(" ", Name, MidName, LastName);
        public string Name { get; private set; }
        public string MidName { get; private set; }
        public string LastName { get; private set; }
        public string Phone { get; private set; }
        internal string Login { get; set; }
        internal string Password { get; set; }
        internal string SecretWord { get; set; }
        public Gender Gender { get; private set; }
    }
}