using System.Diagnostics.SymbolStore;

namespace ÄffarsModelDemo
{
    public class Program
    {
        static void Main(string[] args)
        {
            
            ButikManager butikM = new ButikManager();
            butikM.LaddaKunder();
            bool run = true;
            while (run)
            {
                Console.WriteLine("===Välkomen till Äffaren===");
                Console.WriteLine("1. Logga in");
                Console.WriteLine("2. Registrera ny kund");
                Console.WriteLine("3. Avsluta");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        
                        Kund inlogningKund = butikM.LoggaIn();
                        if (inlogningKund != null)
                        {
                            bool kör = true;
                            while (kör)
                            {
                                Console.WriteLine("======Välkomen i Butiken=====");

                                Console.WriteLine("[1] Handla");
                                Console.WriteLine("[2] Kundvagn");
                                Console.WriteLine("[3] Kassa");
                                Console.WriteLine("[4] Logga ut");
                                Console.WriteLine("[5] Visa Kundens Information");

                                int svar = int.Parse(Console.ReadLine());

                                switch (svar)
                                {
                                    case 1:
                                        butikM.Handla(inlogningKund);
                                        break;
                                    case 2:
                                        butikM.VisaKundvagn(inlogningKund);
                                        break;
                                    case 3:
                                        butikM.Kassa(ref inlogningKund);
                                        break;
                                    case 4:

                                        Console.WriteLine("Du loggar ut");
                                        kör = false;
                                        break;
                                    case 5:
                                        Console.WriteLine(inlogningKund);
                                        break;
                                    default:
                                        Console.WriteLine("fel val");
                                        break;
                                        
                                }
                            }
                        }
                       
                        
                        break;
                    case "2":
                        butikM.RegestrKund();
                        break;
                    case "3":
                        run = false;
                        break;
                    default:
                        Console.WriteLine("Fel val, försök igen");
                        break;
                }


            }







        }
    }
}
