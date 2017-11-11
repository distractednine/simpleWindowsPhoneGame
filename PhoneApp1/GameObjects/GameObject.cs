using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PhoneApp1
{
    internal abstract class GameObject : ViewModelBase
    {
        private Double _left;
        private Double _top;
        private SolidColorBrush _brush;
        private Boolean _isActive;

        public Double Left
        {
            get { return _left; }
            set
            {
                if (value.Equals(_left)) return;
                _left = value;
                OnPropertyChanged();
            }
        }

        public Double Top
        {
            get { return _top; }
            set
            {
                if (value.Equals(_top)) return;
                _top = value;
                OnPropertyChanged();
            }
        }

        public SolidColorBrush Brush
        {
            get { return _brush; }
            set
            {
                if (Equals(value, _brush)) return;
                _brush = value;
                OnPropertyChanged();
            }
        }

        public Boolean IsActive
        {
            get { return _isActive; }
            set
            {
                if (value.Equals(_isActive)) return;
                _isActive = value;
                OnPropertyChanged();
            }
        }
    }
}
