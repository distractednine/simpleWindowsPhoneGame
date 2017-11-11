using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;

namespace PhoneApp1
{
    public partial class EnemyUserControl : UserControl
    {
        public static readonly DependencyProperty EnemyColorProperty =
            DependencyProperty.Register(
            "EnemyColor", 
            typeof(Brush),
            typeof(EnemyUserControl), 
            new PropertyMetadata(
                new SolidColorBrush(Colors.Transparent),
                new PropertyChangedCallback(OnEnemyColorChanged))
            );

        public static readonly DependencyProperty EnemySecondaryColorProperty =
                DependencyProperty.Register(
                "EnemySecondaryColor",
                typeof(Brush),
                typeof(EnemyUserControl),
                new PropertyMetadata(new SolidColorBrush(Colors.Transparent))
                );

        public EnemyUserControl()
        {
            InitializeComponent();
        }

        public Brush EnemyColor
        {
            get { return (Brush)GetValue(EnemyColorProperty); }
            set { SetValue(EnemyColorProperty, value); }
        }

        public Brush EnemySecondaryColor
        {
            get { return (Brush)GetValue(EnemySecondaryColorProperty); }
            set { SetValue(EnemySecondaryColorProperty, value); }
        }

        private static void OnEnemyColorChanged(DependencyObject d, 
            DependencyPropertyChangedEventArgs e)
        {
            var control = (EnemyUserControl)d;
            control.InvalidateSecondaryColor();
        }

        private void InvalidateSecondaryColor()
        {
            var primaryColor =
                (SolidColorBrush)GetValue(EnemyColorProperty);

            var brushColor =
                (primaryColor.Color != Colors.Transparent) ?
                    Colors.Yellow : Colors.Transparent;

            EnemySecondaryColor = new SolidColorBrush(brushColor);
        }
    }
}
