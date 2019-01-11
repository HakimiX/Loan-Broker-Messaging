using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aggregator
{
    class LoanResponse
    {
        int counter = 0;
        double interestRate;
        string fromBank;
        public LoanResponse(double interestRate, string fromBank)
        {
            this.InterestRate = interestRate;
            this.FromBank = fromBank;
        }
        public int Counter
        {
            get { return counter; }
            set { counter = value; }
        }
        public double InterestRate
        {
            get { return interestRate; }
            set { interestRate = value; }
        }
        public string FromBank
        {
            get { return fromBank; }
            set { fromBank = value; }
        }
    }
}
