using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Taxi.Models.Persons
{
    public enum ClientLevel : byte
    {
        New,
        Standard,
        VIP
    }
    public sealed class Client : Person
    {
        public ClientLevel ClientLevel { get; private set; }
    }
}