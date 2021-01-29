using DBService1.Entity;
using SITConnect.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Xml.Serialization;

namespace DBService1
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class Service1 : IService1
    {
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        public CreditCard CreateCard(string name, string num, string cvv, string expiry)
        {
            CreditCard cc = new CreditCard(name, num, cvv, expiry);
            return cc;
        }

        //public string EncryptCard(CreditCard card)
        //{
        //    System.IO.TextWriter cc2 = new System.IO.TextWriter;
        //    XmlSerializer serializer = new XmlSerializer(card.GetType());
        //    serializer.Serialize(cc2, card);
        //    return cc2.ToString();
        //}

        public int CreateAccount(string email, string pw, string pwsalt, string fname, string lname, DateTime dob, string card_name, string card_num, string card_cvv, string card_expiry, byte[] iv, byte[] key)
        {
            User user = new User(email, pw, pwsalt, fname, lname, dob, new CreditCard(card_name, card_num, card_cvv, card_expiry), iv, key);
            return user.Insert();
        }

        public User SelectByEmail(string email)
        {
            User user = new User();
            return user.SelectByEmail(email);
        }

        public int ChangePassword(string email, string newpass)
        {
            User user = new User();
            return user.ChangePassword(email, newpass);
        }

        public bool CheckAttempts(string email, bool pass)
        {
            User user = new User();
            return user.CheckAttempts(email, pass);
        }

        public bool CheckSuspended(string email)
        {
            User user = new User();
            return user.CheckSuspended(email);
        }
    }
}
