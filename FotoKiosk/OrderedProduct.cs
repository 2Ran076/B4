using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotoKiosk
{
    internal class OrderedProduct
    {
        public int FotoNummer { get; set; }

        public string ProductNaam { get; set; }

        public int Aantal { get; set; }

        public decimal TotaalPrijs { get; set; }
    }
}
