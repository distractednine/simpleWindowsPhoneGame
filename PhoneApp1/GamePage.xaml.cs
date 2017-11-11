using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media.Animation;
using System.Windows.Input;
using System.Windows.Threading;

namespace PhoneApp1
{
    public partial class GamePage : PhoneApplicationPage
    {
        private Storyboard _storyboard;
        private Func<Action, DispatcherOperation> _dispatcherFunction;

        public GamePage()
        {
            InitializeComponent();

            _dispatcherFunction = GameCanvas.Dispatcher.BeginInvoke;
            _storyboard = new Storyboard();

            InitializeGame();

            LaunchGameStart();
        }

        internal Game ViewModel
        {
            get { return (Game)DataContext; }
            set { DataContext = value; }
        }

        private void InitializeGame()
        {
            ViewModel = new Game(_dispatcherFunction);
            ViewModel.OnWin += OnWin;
            ViewModel.OnLose += OnLose;
        }

        private void LaunchGameStart()
        {
            PlayAnimation(new Thickness(0, 0, 0, 0), 
                "GameStartAnimation", StartGame);
        }

        private void StartGame(Object sender, EventArgs e)
        {
            ViewModel.StartGame();
        }

        private void OnWin(Object sender, EventArgs e)
        {
            PlayAnimation(new Thickness(-130, 0, 0, 0), 
                "VictoryAnimation", OpenMainMenu);
        }

        private void OpenMainMenu(Object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void OnLose(Object sender, EventArgs e)
        {
            PlayAnimation(new Thickness(-160, 0, 0, 0), "GameOverAnimation", OpenMainMenu);
        }

        private void PlayAnimation(Thickness textMargin, String animationResource,
            EventHandler onCompleted)
        {
            _dispatcherFunction(() =>
            {
                MainMessage.Margin = textMargin;

                _storyboard = (Storyboard)App.Current.Resources[animationResource];
                _storyboard.Stop();
                Storyboard.SetTarget(_storyboard, MainMessage);
                _storyboard.Completed += onCompleted;
                _storyboard.Begin();
            });
        }
    }
}