using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NormalizerJSON
{
    public class jsonResponse
    {
        string SSN;
        double INTERESTRATE;

        public jsonResponse()
        {
        }

        public jsonResponse(double interestRate, string ssn)
        {
            this.SSN = ssn;
            this.INTERESTRATE = interestRate;
        }

        public double interestRate
        {
            get { return INTERESTRATE; }
            set { INTERESTRATE = value; }
        }

        public string ssn
        {
            get { return SSN; }
            set { SSN = value; }
        }
    }
}
