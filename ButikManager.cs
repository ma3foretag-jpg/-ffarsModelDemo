using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ÄffarsModelDemo
{
    public class ButikManager
    {
        string filväg = "kunder.txt";
        List<Kund> kunder = new List<Kund>(); 
        List<Produkt> produkter = new List<Produkt>() 
        { new("Korv", 10,0),
            new("Äpple", 20, 0),
            new("Dricka", 15, 0)
        };

        Dictionary<string, double> valutakurser = new Dictionary<string, double>()
        {
            {"SEK", 1.0 },
            {"EUR", 0.088 },
            {"USD", 0.095 }
        };
        private string valdValuta = "SEK";
        private double kurs = 1.0;

        public void LaddaKunder()
        {
            if (!File.Exists(filväg))
            {
                return;
            }
                string[] rader = File.ReadAllLines(filväg);
            foreach (string rad in rader)
            {
                if (string.IsNullOrWhiteSpace(rad))
                {
                    continue;
                }

                string[] delar = rad.Split(',');

                if (delar.Length < 3)
                {
                    continue;
                }

                string name = delar[0];
                string lösenord = delar[1];
                string typ = delar[2];

                Kund kund;

                switch (typ)
                {
                    case "Gold":
                        kund = new Gold(name, lösenord);
                        break;
                    case "Silver":
                        kund = new Silver(name, lösenord);
                        break;
                    case "Bronze":
                        kund = new Bronze(name, lösenord);
                        break;
                    default:
                        kund = new Kund(name, lösenord);
                        break;

                }
                kunder.Add(kund);
            }
        }

        public void SparaKunder()
        {
            List<string> rader = new List<string>();

            foreach (Kund k in kunder)
            {
                string typ = k.GetType().Name;
                rader.Add($"{k.Name},{k.Lösenord},{typ}");
            }

            File.WriteAllLines(filväg, rader);
        }
        public Kund LoggaIn()
        {
            Console.Write("Ange ditt namn: ");
            string name = Console.ReadLine();
            Kund hittadKund = null;
            foreach (Kund k in kunder)
            {
                if (k.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    hittadKund = k;
                    break;
                }
            }
            if (hittadKund == null)
                
                {
                    Console.WriteLine("Du är inte regesrerat! vill du regestrera dig (ja/nej)");
                    string svar = Console.ReadLine();

                    if (svar.ToLower() == "ja")
                    {
                        RegestrKund();
                    }
                    return null;
                }
            
            int försök = 3;
            while (försök>0)
            {
                Console.Write("Ange ditt lösenord: ");
                string lösenord = Console.ReadLine();

                if (hittadKund.VeriferLösenord(lösenord) )
                {
                    Console.WriteLine($"Välkomen {hittadKund.Name}");
                    return hittadKund;
                }
                else
                {
                    försök--;
                    Console.WriteLine($"Fel lösenord! du har {försök} försök kvar");
                }
            }
            Console.WriteLine("Du har inga försök kvar, programmet avslutas");
            return null;
        }

        public void RegestrKund()
        {
            Console.Write("Ange ditt namn: ");
            string name = Console.ReadLine();
            Console.Write("Ange ditt lösenord: ");
            string lösenord = Console.ReadLine();

            bool finnsredan = false;
            foreach (Kund k in kunder)
            {
                if (k.Name == name)
                {
                   finnsredan = true;
                   Console.WriteLine("Kunden finns redan! Välje ett annat namn");
                   break;

                }
            }
            if (!finnsredan)
            {
                kunder.Add(new Kund(name, lösenord));
                Console.WriteLine("Kunden är regestrerad");

            }

            SparaKunder();
        }

        public void VisaValuta()
        {
            Console.WriteLine("Välje valuta: [1]- SEK   [2]- EUR   [3]- USD");
            int val = int.Parse(Console.ReadLine());
            valdValuta = "SEK";
            kurs = 1.0;
            if (val == 2)
            {
                valdValuta = "EUR";
                kurs = valutakurser["EUR"];
            }
            else if (val == 3)
            {
                valdValuta = "USD";
                kurs = valutakurser["USD"];
            }
            Console.WriteLine($"\nPriser visas i {valdValuta}:");
        }

        public void VisaVaror()
        {
            VisaValuta();
            int i = 1;
            foreach (var varor in produkter)
            {
                double prisMedKurs = varor.Preis * kurs;
                Console.WriteLine($"[{i}] {varor.Name} - {prisMedKurs:F2} {valdValuta}");
                i++;
            }
        }

        public void Handla(Kund kund)
        {
            Console.WriteLine("Vad vill du köpa idag? \n (Ange 0 om du avslutar)");
            VisaVaror();
            int svar =int.Parse( Console.ReadLine());

            if (svar == 0)
            {
                Console.WriteLine("Avsluta köp");
                return;
            }
            if(svar < 1 || svar > produkter.Count)
            {
                Console.WriteLine("Ogiltigt val, försök igen");
                return;
            }

            Produkt valdProdukt = produkter[svar - 1];

            Console.WriteLine($"Hur många {valdProdukt.Name} vill du köpa? ");
            int antal = int.Parse(Console.ReadLine());

            Produkt kundProdukt = new Produkt(valdProdukt.Name, valdProdukt.Preis, antal);
           
            kund.Kundvagn.Add(kundProdukt);

            Console.WriteLine($"{antal} st {valdProdukt.Name} har lagts till i ditt kundvagn");
        }

        public void VisaKundvagn(Kund kund)
        {
            if (kund.Kundvagn.Count == 0) { 
                Console.WriteLine("Din kundvagn är tom.  tryck någon knpp att förstsätta!");
                Console.ReadKey();
                Console.Clear();
                return;
            }
            VisaValuta();
            double totalt = 0;
            Console.WriteLine("Din kundvagn innehåller: ");

            foreach (Produkt p in kund.Kundvagn)
            {
                totalt += p.TotalPrice();
                Console.WriteLine($"{p.Name} -  {p.Preis * kurs:F2}{valdValuta}st  -  {p.Antal}st  - {p.TotalPrice() * kurs:F2} {valdValuta}");
            }
            Console.WriteLine($"Totalt att betala: {totalt}kr");
        }

        public void Kassa(ref Kund kund)
        {
            if (kund.Kundvagn.Count==0)
            {
                Console.WriteLine("Din kundvagn är tom");
            }
            double totaltAttBetala = 0;
            foreach (var p in kund.Kundvagn)
            {
                totaltAttBetala += p.TotalPrice();
                Console.WriteLine($"{p.Name}\t {p.Preis}kr/st\t Antal:{p.Antal} \t {p.TotalPrice()}kr");
            }
            kund = UppgraderaKund(kund);
            double PrisEfterRabatt = kund.Rabatt();
            Console.WriteLine($"----------------------------\n Totalt att betala :{totaltAttBetala}kr  \nEfter rabat du ska betala {PrisEfterRabatt}");
           

            Console.WriteLine("Tack för ditt köp");

            

            //resnar jag kundvagene äfter betalt
            kund.Kundvagn.Clear();
        }


        public Kund UppgraderaKund(Kund kund)
        {
            double total = kund.Kundvagn.Sum(p => p.TotalPrice());

            Kund nykund = kund;
            if (total >= 100)
            {
                nykund = new Gold(kund.Name, kund.Lösenord);
            }else if (total >= 50)
            {
                nykund = new Silver(kund.Name, kund.Lösenord);
            }else if (total >= 20)
            {
                nykund = new Bronze(kund.Name, kund.Lösenord);
            }

            if (nykund != kund)
            {
                nykund.Kundvagn = kund.Kundvagn;
            }

            return nykund;
        }

    }
}
