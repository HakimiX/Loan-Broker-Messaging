using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslatorJSON
{
    class JsonRequest
    {
        string SSN;
        int CREDITSCORE;
        double LOANAMOUNT;
        int LOANDURATION;

        public JsonRequest()
        {
        }

        public JsonRequest(string ssn, int creditScore, double loanAmount, int loanDuration)
        {
            SSN = ssn;
            CREDITSCORE = creditScore;
            LOANAMOUNT = loanAmount;
            LOANDURATION = loanDuration;
        }

        public string ssn
        {
            get { return SSN; }
            set { SSN = value; }
        }


        public int creditScore
        {
            get { return CREDITSCORE; }
            set { CREDITSCORE = value; }
        }

        public double loanAmount
        {
            get { return LOANAMOUNT; }
            set { LOANAMOUNT = value; }
        }

        public int loanDuration
        {
            get { return LOANDURATION; }
            set { LOANDURATION = value; }
        }
    }
}
