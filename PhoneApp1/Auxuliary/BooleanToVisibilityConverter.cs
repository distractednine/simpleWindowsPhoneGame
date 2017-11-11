using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace PhoneApp1
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public Object Convert(Object value, Type targetType, 
            Object parameter, System.Globalization.CultureInfo culture)
        {
            var bValue = (Boolean)value;
            if (bValue)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }

        public Object ConvertBack(Object value, Type targetType, 
            Object parameter, System.Globalization.CultureInfo culture)
        {
            Visibility visibility = (Visibility)value;

            if (visibility == Visibility.Visible)
                return true;
            else
                return false;
        }
    }
}
