﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TranslatorWS.ServiceReferences {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReferences.IService1")]
    public interface IService1 {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/GetData", ReplyAction="http://tempuri.org/IService1/GetDataResponse")]
        string GetData(int value);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/GetData", ReplyAction="http://tempuri.org/IService1/GetDataResponse")]
        System.Threading.Tasks.Task<string> GetDataAsync(int value);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/GetDataUsingDataContract", ReplyAction="http://tempuri.org/IService1/GetDataUsingDataContractResponse")]
        ServiceApp.CompositeType GetDataUsingDataContract(ServiceApp.CompositeType composite);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/GetDataUsingDataContract", ReplyAction="http://tempuri.org/IService1/GetDataUsingDataContractResponse")]
        System.Threading.Tasks.Task<ServiceApp.CompositeType> GetDataUsingDataContractAsync(ServiceApp.CompositeType composite);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/getName", ReplyAction="http://tempuri.org/IService1/getNameResponse")]
        string getName(string name);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/getName", ReplyAction="http://tempuri.org/IService1/getNameResponse")]
        System.Threading.Tasks.Task<string> getNameAsync(string name);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/getAge", ReplyAction="http://tempuri.org/IService1/getAgeResponse")]
        int getAge(int age);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/getAge", ReplyAction="http://tempuri.org/IService1/getAgeResponse")]
        System.Threading.Tasks.Task<int> getAgeAsync(int age);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/getBanks", ReplyAction="http://tempuri.org/IService1/getBanksResponse")]
        ClassEngine.Bank[] getBanks(int creditScore, double amount, int duration);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/getBanks", ReplyAction="http://tempuri.org/IService1/getBanksResponse")]
        System.Threading.Tasks.Task<ClassEngine.Bank[]> getBanksAsync(int creditScore, double amount, int duration);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/getInterestRate", ReplyAction="http://tempuri.org/IService1/getInterestRateResponse")]
        void getInterestRate(string ssn, double amount, int duration, int creditScore, string replyToAddress);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/getInterestRate", ReplyAction="http://tempuri.org/IService1/getInterestRateResponse")]
        System.Threading.Tasks.Task getInterestRateAsync(string ssn, double amount, int duration, int creditScore, string replyToAddress);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/ResponseMessageToQueue", ReplyAction="http://tempuri.org/IService1/ResponseMessageToQueueResponse")]
        void ResponseMessageToQueue(string message, string replyAddress);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/ResponseMessageToQueue", ReplyAction="http://tempuri.org/IService1/ResponseMessageToQueueResponse")]
        System.Threading.Tasks.Task ResponseMessageToQueueAsync(string message, string replyAddress);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/LoanRequest", ReplyAction="http://tempuri.org/IService1/LoanRequestResponse")]
        string LoanRequest(string ssn, double amount, int duration);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/LoanRequest", ReplyAction="http://tempuri.org/IService1/LoanRequestResponse")]
        System.Threading.Tasks.Task<string> LoanRequestAsync(string ssn, double amount, int duration);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/sendMessage", ReplyAction="http://tempuri.org/IService1/sendMessageResponse")]
        void sendMessage(string EXCHANGE_NAME, string message);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/sendMessage", ReplyAction="http://tempuri.org/IService1/sendMessageResponse")]
        System.Threading.Tasks.Task sendMessageAsync(string EXCHANGE_NAME, string message);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IService1Channel : TranslatorWS.ServiceReferences.IService1, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class Service1Client : System.ServiceModel.ClientBase<TranslatorWS.ServiceReferences.IService1>, TranslatorWS.ServiceReferences.IService1 {
        
        public Service1Client() {
        }
        
        public Service1Client(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public Service1Client(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public Service1Client(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public Service1Client(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string GetData(int value) {
            return base.Channel.GetData(value);
        }
        
        public System.Threading.Tasks.Task<string> GetDataAsync(int value) {
            return base.Channel.GetDataAsync(value);
        }
        
        public ServiceApp.CompositeType GetDataUsingDataContract(ServiceApp.CompositeType composite) {
            return base.Channel.GetDataUsingDataContract(composite);
        }
        
        public System.Threading.Tasks.Task<ServiceApp.CompositeType> GetDataUsingDataContractAsync(ServiceApp.CompositeType composite) {
            return base.Channel.GetDataUsingDataContractAsync(composite);
        }
        
        public string getName(string name) {
            return base.Channel.getName(name);
        }
        
        public System.Threading.Tasks.Task<string> getNameAsync(string name) {
            return base.Channel.getNameAsync(name);
        }
        
        public int getAge(int age) {
            return base.Channel.getAge(age);
        }
        
        public System.Threading.Tasks.Task<int> getAgeAsync(int age) {
            return base.Channel.getAgeAsync(age);
        }
        
        public ClassEngine.Bank[] getBanks(int creditScore, double amount, int duration) {
            return base.Channel.getBanks(creditScore, amount, duration);
        }
        
        public System.Threading.Tasks.Task<ClassEngine.Bank[]> getBanksAsync(int creditScore, double amount, int duration) {
            return base.Channel.getBanksAsync(creditScore, amount, duration);
        }
        
        public void getInterestRate(string ssn, double amount, int duration, int creditScore, string replyToAddress) {
            base.Channel.getInterestRate(ssn, amount, duration, creditScore, replyToAddress);
        }
        
        public System.Threading.Tasks.Task getInterestRateAsync(string ssn, double amount, int duration, int creditScore, string replyToAddress) {
            return base.Channel.getInterestRateAsync(ssn, amount, duration, creditScore, replyToAddress);
        }
        
        public void ResponseMessageToQueue(string message, string replyAddress) {
            base.Channel.ResponseMessageToQueue(message, replyAddress);
        }
        
        public System.Threading.Tasks.Task ResponseMessageToQueueAsync(string message, string replyAddress) {
            return base.Channel.ResponseMessageToQueueAsync(message, replyAddress);
        }
        
        public string LoanRequest(string ssn, double amount, int duration) {
            return base.Channel.LoanRequest(ssn, amount, duration);
        }
        
        public System.Threading.Tasks.Task<string> LoanRequestAsync(string ssn, double amount, int duration) {
            return base.Channel.LoanRequestAsync(ssn, amount, duration);
        }
        
        public void sendMessage(string EXCHANGE_NAME, string message) {
            base.Channel.sendMessage(EXCHANGE_NAME, message);
        }
        
        public System.Threading.Tasks.Task sendMessageAsync(string EXCHANGE_NAME, string message) {
            return base.Channel.sendMessageAsync(EXCHANGE_NAME, message);
        }
    }
}
