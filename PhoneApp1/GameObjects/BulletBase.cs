using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PhoneApp1.GameObjects
{
    internal abstract class BulletBase : GameObject
    {
        protected BulletBase()
        {
            Reset();
        }

        public void Reset()
        {
            IsActive = false;
            Left = ScreenConfigurations.LeftCorner;
            Top = ResetTop();
            Brush = new SolidColorBrush(Colors.Transparent);
        }

        public void Move()
        {
            if (Left == 0) return;

            IsActive = true;
            Top = MoveTop();
            Brush = new SolidColorBrush(ColorProvider.GetRandomColor());
        }

        protected virtual Double ResetTop() 
        {
            return 0;
        }

        protected abstract Double MoveTop();
    }
}
