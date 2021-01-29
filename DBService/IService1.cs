using DBService1.Entity;
using SITConnect.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace DBService1
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        // TODO: Add your service operations here
        [OperationContract]
        CreditCard CreateCard(string name, string num, string cvv, string expiry);

        //[OperationContract]
        //string EncryptCard(CreditCard card);

        [OperationContract]
        int CreateAccount(string email, string pw, string pwsalt, string fname, string lname, DateTime dob, string card_name, string card_num, string card_cvv, string card_expiry, byte[] iv, byte[] key);

        [OperationContract]
        User SelectByEmail(string email);

        [OperationContract]
        int ChangePassword(string email, string newpass);

        [OperationContract]
        bool CheckAttempts(string email, bool pass);

        [OperationContract]
        bool CheckSuspended(string email);

    }

    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    // You can add XSD files into the project. After building the project, you can directly use the data types defined there, with the namespace "DBService1.ContractType".
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}
