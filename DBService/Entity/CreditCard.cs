using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace SITConnect.Entity
{
    public class CreditCard
    {
        public string Name { get; set; }
        public string Number { get; set; }
        public string Cvv { get; set; }
        public string Expiry { get; set; }

        public CreditCard()
        {

        }

        public CreditCard(string name, string num, string cvv, string expiry)
        {
            Name = name;
            Number = num;
            Cvv = cvv;
            Expiry = expiry;
        }
    }
}