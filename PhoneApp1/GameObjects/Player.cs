using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PhoneApp1
{
    internal class Player : GameObject
    {
        public Player()
        {
            IsActive = true;
            Top = ScreenConfigurations.PlayerTop;
            Left = ScreenConfigurations.CenterScreen;
        }

        public void MoveLeft(Object sender)
        {
            if (Left < ScreenConfigurations.LeftCorner) return;

            Left = Left - GameConfigurations.PlayerMoveDistance;
        }

        public void MoveRight(Object sender)
        {
            if (Left > ScreenConfigurations.RightCorner) return;

            Left = Left + GameConfigurations.PlayerMoveDistance;
        }
    }
}
