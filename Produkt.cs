using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ÄffarsModelDemo
{
    public class Produkt
    {
        public string Name { get;  set; }
        public int Preis { get; set; }

        public int Antal { get; set; }


        public Produkt(string name, int preis, int antal )
        {
            Name = name;
            Preis = preis;
            Antal = antal;

        }
        

        public double TotalPrice()
        {
            return Preis * Antal;
        }
    }

    
}
