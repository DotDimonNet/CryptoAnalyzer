using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAnalyzer
{
    public class CryptoRow
    {
        public DateTime Date { get; set; }
        public double MaxRate { get; set; }
        public double MinRate { get; set; }
        public double EndRate { get; set; }
    }
}
