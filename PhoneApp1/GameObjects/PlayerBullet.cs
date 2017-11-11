using PhoneApp1.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PhoneApp1
{
    internal class PlayerBullet : BulletBase
    {
        protected override Double ResetTop() 
        {
            return ScreenConfigurations.PlayerTop;
        }

        protected override Double MoveTop() 
        {
            return Top - GameConfigurations.BulletDistance;
        }
    }
}
