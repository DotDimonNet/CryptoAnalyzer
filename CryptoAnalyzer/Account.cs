using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAnalyzer
{
    public class Account
    {
        public double BalanceBTC { get; set; }
        public double BalanceUSD { get; set; }
        public double TreshholdBTCtoUSD { get; set; }
        public double TreshholdUSDtoBTC { get; set; }
        public double MaxActionFromTotalinPercent { get; set; }
    }
}
