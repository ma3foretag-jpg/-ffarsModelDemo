using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ÄffarsModelDemo
{
    public class ButikManager
    {
        List<Kund> kunder = new List<Kund>(); 
        List<Produkt> produkter = new List<Produkt>() 
        { new("Korv", 10),
            new("Äpple", 20),
            new("Dricka", 15)
        };


       
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


        }

        public void VisaVaror()
        {
            int i = 1;
            foreach (var varor in produkter)
            {
                Console.WriteLine($"[{i}] {varor.Name} - {varor.Preis}st/kr");
                i++;
            }
        }

        public void Handla(Kund kund)
        {
            VisaVaror();
            Console.WriteLine("Vad vill du köpa idag? [För att få goldrabatt du ska köpa mer än 100kr]\n (Ange 0 om du avslutar)");
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

            Produkt kundProdukt = new Produkt
            {
                Name = valdProdukt.Name,
                Preis = valdProdukt.Preis,
                Antal = antal
                
            };
           
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
            double totalt = 0;
            Console.WriteLine("Din kundvagn innehåller: ");

            foreach (Produkt p in kund.Kundvagn)
            {
                totalt += p.TotalPrice();
                Console.WriteLine($"{p.Name} - {p.Preis}st/kr - Antal:{p.Antal} - {p.TotalPrice()}kr");
            }
            Console.WriteLine($"Totalt att betala: {totalt}kr");
        }

        public void Kassa(Kund kund)
        {
            if (kund.Kundvagn.Count==0)
            {
                Console.WriteLine("Din kundvagn är tom");
            }
            double totaltAttBetala = 0;
            foreach (var p in kund.Kundvagn)
            {
                totaltAttBetala += p.TotalPrice();
                Console.WriteLine($"{p.Name}\t {p.Preis}kr/st\t {p.TotalPrice()}kr");
            }
            Console.WriteLine($"----------------------------\n Totalt att betala :{totaltAttBetala}kr");
           
            
            Console.WriteLine("Tack för ditt köp");

            kund.TotalKöp += totaltAttBetala;

            //resnar jag kundvagene äfter betalt
            kund.Kundvagn.Clear();
        }


    }
}
