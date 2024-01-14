using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotoKiosk
{
    internal class GlobaalReport
    {
        public string PhotoId { get; set; }
        public string Product { get; set; }
        public double Amount { get; set; }
        public double TotalPrice { get; set; }
        public double OrderCount { get; set; }
        public double PurchaseRatio { get; set; }
        public string Day { get; set; }
    }
}
