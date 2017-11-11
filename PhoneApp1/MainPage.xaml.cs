using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using PhoneApp1.Resources;
using System.Windows.Media.Animation;
using System.Threading;
using System.Windows.Threading;
using System.Windows.Input;
using PhoneApp1.Auxuliary;
using PhoneApp1.Config;

namespace PhoneApp1
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();

            DataContext = this;

            BackgroundManager = BackgroundProvider.BackgroundManager;
            EnemyConfigurations = ConfigurationsProvider.EnemyConfigurations;
        }

        public BackgroundManager BackgroundManager { get; set; }
        public EnemyConfigurations EnemyConfigurations { get; set; }

        private void StartGame_Tap(Object sender, GestureEventArgs e)
        {
            App.RootFrame.Navigate(new Uri("/GamePage.xaml", UriKind.Relative));
        }

        private void AuthorTextBlock_Tap(Object sender, GestureEventArgs e)
        {
            try
            {
                var sb = (Storyboard)App.Current.Resources["AuthorAnimation"];
                Storyboard.SetTarget(sb, AuthorTextBlockRotateTransform);
                sb.Begin();
            }
            catch (Exception) { }
        }

        private void ButtonDifficulty_Tap(Object sender, GestureEventArgs e)
        {
            EnemyConfigurations.ToggleDifficultyLevel();
        }

        private void ButtonExit_Tap(Object sender, GestureEventArgs e)
        {
            App.Current.Terminate();
        }
    }
}