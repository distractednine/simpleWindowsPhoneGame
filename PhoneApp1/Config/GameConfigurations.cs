using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneApp1
{
    internal class GameConfigurations
    {
        public static Double PlayerWidth
        {
            get
            {
                return 90;
            }
        }

        public static Double PlayerHeight
        {
            get
            {
                return 100;
            }
        }

        public static Double PlayerMoveDistance
        {
            get
            {
                return 15;
            }
        }

        public static Double BulletDistance
        {
            get
            {
                return 5;
            }
        }

        public static Int32 BulletSpeed
        {
            get
            {
                return 20;
            }
        }

        public static Double ButtonSpeed
        {
            get
            {
                return 5;
            }
        }

        public static Double EnemyWidth
        {
            get
            {
                return 80;
            }
        }

        public static Double EnemyHeight
        {
            get
            {
                return 30;
            }
        }

        public static Double MovesNumberToChangeTheme
        {
            get
            {
                return 17;
            }
        }
    }
}
