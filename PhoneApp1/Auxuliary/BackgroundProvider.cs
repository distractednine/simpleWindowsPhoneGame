using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneApp1.Auxuliary
{
    public static class BackgroundProvider
    {
        public static BackgroundManager BackgroundManager { get; private set; }

        static BackgroundProvider() 
        {
            BackgroundManager = new BackgroundManager();
        }
    }
}
