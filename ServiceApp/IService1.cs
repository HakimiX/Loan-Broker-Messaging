using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using ClassEngine;

namespace ServiceApp
{
    [ServiceContract]
    public interface IService1
    {

        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        [OperationContract]
        string getName(string name);

        [OperationContract]
        int getAge(int age);

        [OperationContract]
        List<Bank> getBanks(int creditScore, double amount, int duration);

        [OperationContract]
        void getInterestRate(string ssn, double amount, int duration, int creditScore, string replyToAddress);

        [OperationContract]
        void ResponseMessageToQueue(string message, string replyAddress);

        [OperationContract]
        string LoanRequest(string ssn, double amount, int duration);

        [OperationContract]
        void sendMessage(string EXCHANGE_NAME, string message);

    }


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
