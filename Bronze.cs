using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ÄffarsModelDemo
{
    public class Bronze : Kund
    {
        
        public Bronze(string name, string lösenord) : base(name, lösenord)
        {

        }

        public override double Rabatt()
        {
            double total = base.Rabatt();
            return total * 0.95;
        }
    }
}
