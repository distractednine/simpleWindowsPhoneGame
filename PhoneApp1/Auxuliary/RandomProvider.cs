using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneApp1
{
    internal class RandomProvider
    {
        static RandomProvider() 
        {
            Random = new Random();
        }

        public static Random Random { get; set; }
    }
}
