using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ÄffarsModelDemo
{
    public class Gold : Kund
    {
        private double rabatt = 0.15;
        public Gold(string name, string lösenord) : base(name, lösenord)
        {
           
        }

       
    }
}
