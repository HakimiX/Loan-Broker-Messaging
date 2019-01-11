using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClassEngine
{
    // DTO Class

    public class Bank
    {
        private int bankIdentificationCode;
        private string bankname;
        private string exchange;
        private string normalizer;
        private string translatorType;

        public Bank(int bankIdentificationCode, string bankname, string exchange, string normalizer, string translatorType)
        {
            this.bankIdentificationCode = bankIdentificationCode;
            this.bankname = bankname;
            this.exchange = exchange;
            this.normalizer = normalizer;
            this.translatorType = translatorType;
        }

        public Bank(string bankname, string exchange)
        {
            this.bankname = bankname;
            this.exchange = exchange;
        }

        public Bank()
        {

        }

        public int BankIdentificationCode
        {
            get { return bankIdentificationCode; }
            set { bankIdentificationCode = value; }
        }

        public string TranslatorType 
        {
            get { return translatorType; }
            set { translatorType = value; }
        }

        public string Normalizer 
        {
            get { return normalizer; }
            set { normalizer = value; }
        }

        public string Bankname 
        {
            get { return bankname; }
            set { bankname = value; }
        }

        public string Exchange
        {
            get { return exchange; }
            set { exchange = value; }
        }

    }
}
