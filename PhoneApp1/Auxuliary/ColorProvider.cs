using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PhoneApp1
{
    static class ColorProvider
    {
        private static readonly Random Random = RandomProvider.Random;

        public static Color GetRandomColor()
        {
            return new Color
            {
                A = (Byte)Random.Next(1, 255),
                R = (Byte)Random.Next(1, 255),
                G = (Byte)Random.Next(1, 255),
                B = (Byte)Random.Next(1, 255)
            };
        }

        public static Color GetEnemyColor()
        {
            var num = Random.Next(0, 100);

            if (num % 7 == 0) return Colors.Purple;
            if (num % 6 == 0) return Colors.Magenta;
            if (num % 5 == 0) return Colors.Red;
            if (num % 4 == 0) return Colors.Blue;
            if (num % 3 == 0) return Colors.Orange;

            return Colors.Green;
        }
    }
}
