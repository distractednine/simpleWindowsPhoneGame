using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PhoneApp1
{
    internal class Enemy : GameObject
    {
        public Enemy()
        {
            IsActive = true;
            Brush = new SolidColorBrush(ColorProvider.GetEnemyColor());
        }

        public void Move(Double left, Double top)
        {
            Left = left;
            Top = top;
        }

        public void Destroy()
        {
            Brush = new SolidColorBrush(Colors.Transparent);
            IsActive = false;
        }
    }
}
