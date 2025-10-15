using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ÄffarsModelDemo
{
    public class Kund
    {
        public string Name { get; private set; }
        public string Lösenord { get; private set; }

        

        public List<Produkt> Kundvagn;

        public Kund( string name, string lösenord)
        {
            Name = name;
            Lösenord = lösenord;
            Kundvagn = new List<Produkt>();
        }


        public bool VeriferLösenord(string lös)
        {
            return lös == Lösenord;
        }

        public override string ToString()
        {
            string KundInfo =$"Kundname: {Name}\nKundLössenord: {Lösenord} \nKundvagn:\n";
            if (Kundvagn.Count == 0)
            {
                KundInfo += "Är tom";
            }
            else
            {
                double total = 0;
                KundInfo += $"ProduktName\tPris\tAntal\tTotal\n"; 
                foreach (var p in Kundvagn)
                {
                    total += p.TotalPrice();

                    KundInfo += $"--------------------------------------------\n{p.Name}  \t{p.Preis}kr/st \t{p.Antal}\t{p.TotalPrice()}\n";
                                
                }
                KundInfo += $"_____________________\nTotalt att betala:{total}";
                Console.WriteLine("Tryck någon knpp att förstsätta!");
                Console.ReadKey();
                Console.Clear();
            }
            return KundInfo;
        }

        public virtual double Rabatt()
        {
            //den heter lambada och genom att använda sum() metoden som är en del av LINQ-biblioteket
            //kan man enkelt beräkna summan av priserna för alla produkter i kundvagnen.
            return Kundvagn.Sum(p => p.TotalPrice());
        }













        }
}

    
