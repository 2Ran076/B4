using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotoKiosk
{
    internal class Report
    {
        public string PhotoId { get; set; }
        public string Product { get; set; }
        public int Amount { get; set; }
        public double TotalPrice { get; set; }
    }
}
