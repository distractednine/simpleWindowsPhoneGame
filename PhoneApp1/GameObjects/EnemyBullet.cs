using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneApp1.GameObjects
{
    internal class EnemyBullet : BulletBase
    {
        protected override Double MoveTop()
        {
            return Top + GameConfigurations.BulletDistance;
        }
    }
}
