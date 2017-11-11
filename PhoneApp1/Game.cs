using Microsoft.Phone.Controls;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using PhoneApp1.GameObjects;
using PhoneApp1.Config;
using System.Windows.Media.Animation;
using PhoneApp1.Auxuliary;

namespace PhoneApp1
{
    internal class Game : ViewModelBase
    {
        #region Constructor

        private Func<Action, DispatcherOperation> _dispatcherFunction;

        public Game(Func<Action, DispatcherOperation> dispatcherFunction)
        {
            _dispatcherFunction = dispatcherFunction;

            _enemyConfigurations = ConfigurationsProvider.EnemyConfigurations;

            BackgroundManager = BackgroundProvider.BackgroundManager;

            InitializePlayer();
            InitializePlayerBullets();

            InitializeEnemies();
            InitializeEnemyBullets();

            UpdateScore();
        }

        public BackgroundManager BackgroundManager { get; set; }

        #endregion

        #region Events

        public event EventHandler OnWin;
        public event EventHandler OnLose;

        #endregion

        #region Game mode

        private volatile Boolean _isActiveGameMode;

        public Boolean IsActiveGameMode
        {
            get { return _isActiveGameMode; }
            set
            {
                if (value.Equals(_isActiveGameMode)) return;

                _isActiveGameMode = value;
                OnPropertyChanged();

                if (FireCommand != null)
                {
                    _dispatcherFunction(MoveLeftCommand.RaiseCanExecuteChanged);
                    _dispatcherFunction(MoveRightCommand.RaiseCanExecuteChanged);
                    _dispatcherFunction(FireCommand.RaiseCanExecuteChanged);
                }
            }
        }

        public void StartGame()
        {
            IsActiveGameMode = true;

            StartEnemyMove();
            StartEnemyAttack();
        }

        private void FinishGame(Boolean isVictory)
        {
            IsActiveGameMode = false;

            _dispatcherFunction(() =>
            {
                PlayerBulletList.ForEach(x => x.Reset());
                EnemyBulletList.ForEach(x => x.Reset());
            });

            if (isVictory && OnWin != null) OnWin(this, new EventArgs());
            if (!isVictory && OnLose != null) OnLose(this, new EventArgs());
        }

        #endregion

        #region Score message

        private String _scoreMessage;

        public String ScoreMessage
        {
            get { return _scoreMessage; }
            set
            {
                if (value.Equals(_scoreMessage)) return;
                _scoreMessage = value;
                OnPropertyChanged();
            }
        }

        private void UpdateScore()
        {
            _dispatcherFunction(() =>
            {
                var score = (20 - ActiveEnemies.Count) * 10;
                ScoreMessage =
                    String.Format("Score: {0}", score);
            });
        }

        #endregion

        #region Player bullets

        public BulletBase PlayerBullet1 { get; set; }
        public BulletBase PlayerBullet2 { get; set; }
        public BulletBase PlayerBullet3 { get; set; }
        public BulletBase PlayerBullet4 { get; set; }
        public BulletBase PlayerBullet5 { get; set; }
        public BulletBase PlayerBullet6 { get; set; }

        private List<BulletBase> PlayerBulletList { get; set; }

        private void InitializePlayerBullets()
        {
            PlayerBullet1 = new PlayerBullet();
            PlayerBullet2 = new PlayerBullet();
            PlayerBullet3 = new PlayerBullet();
            PlayerBullet4 = new PlayerBullet();
            PlayerBullet5 = new PlayerBullet();
            PlayerBullet6 = new PlayerBullet();

            PlayerBulletList =
                new List<BulletBase> { PlayerBullet1, PlayerBullet2, PlayerBullet3, PlayerBullet4, 
                    PlayerBullet5, PlayerBullet6};
        }

        #endregion

        #region Player fire

        public Player Player { get; set; }

        public DelegateCommand MoveLeftCommand { get; private set; }
        public DelegateCommand MoveRightCommand { get; private set; }
        public DelegateCommand FireCommand { get; private set; }

        private void InitializePlayer()
        {
            Player = new Player();
            MoveLeftCommand = new DelegateCommand(Player.MoveLeft, AreControlsEnabled);
            MoveRightCommand = new DelegateCommand(Player.MoveRight, AreControlsEnabled);
            FireCommand = new DelegateCommand(Fire, AreControlsEnabled);
        }

        private async void Fire(Object sender)
        {
            var bullet = GetInactiveBullet(PlayerBulletList);
            if (bullet == null) return;

            await Task.Run(() =>
            {
                _dispatcherFunction(() => bullet.Left = GetBulletLeft());

                while (bullet.Top > ScreenConfigurations.TopCorner)
                {
                    _dispatcherFunction(bullet.Move);

                    var hitEnemy = CheckEnemyHit(bullet);

                    if (hitEnemy != null)
                    {
                        _dispatcherFunction(() =>
                        {
                            hitEnemy.Destroy();
                            bullet.Reset();
                        });
                        UpdateScore();

                        if (!ActiveEnemies.Any()) FinishGame(true);

                        return;
                    }

                    Thread.Sleep(GameConfigurations.BulletSpeed);
                }

                _dispatcherFunction(bullet.Reset);
            });
        }

        private Boolean AreControlsEnabled(Object sender)
        {
            return IsActiveGameMode;
        }

        private Enemy CheckEnemyHit(BulletBase bullet)
        {
            var hitEnemy =
                ActiveEnemies.FirstOrDefault(
                    x =>
                        (bullet.Left >= x.Left &&
                            bullet.Left <= x.Left + GameConfigurations.EnemyWidth)
                        &&
                        (bullet.Top >= x.Top &&
                            bullet.Top <= x.Top + GameConfigurations.EnemyHeight));

            return hitEnemy;
        }

        private BulletBase GetInactiveBullet(List<BulletBase> bulletList)
        {
            return bulletList.FirstOrDefault(x => x.IsActive == false);
        }

        private Double GetBulletLeft()
        {
            return Player.Left + GameConfigurations.PlayerWidth / 2;
        }

        #endregion

        #region Enemy bullets

        public BulletBase EnemyBullet1 { get; set; }
        public BulletBase EnemyBullet2 { get; set; }
        public BulletBase EnemyBullet3 { get; set; }

        private List<BulletBase> EnemyBulletList { get; set; }

        private void InitializeEnemyBullets()
        {
            EnemyBullet1 = new EnemyBullet();
            EnemyBullet2 = new EnemyBullet();
            EnemyBullet3 = new EnemyBullet();

            EnemyBulletList =
                new List<BulletBase> { EnemyBullet1, EnemyBullet2, EnemyBullet3};
        }

        #endregion

        #region Enemies

        private EnemyConfigurations _enemyConfigurations;

        public Enemy Enemy1 { get; set; }
        public Enemy Enemy2 { get; set; }
        public Enemy Enemy3 { get; set; }
        public Enemy Enemy4 { get; set; }
        public Enemy Enemy5 { get; set; }
        public Enemy Enemy6 { get; set; }
        public Enemy Enemy7 { get; set; }
        public Enemy Enemy8 { get; set; }
        public Enemy Enemy9 { get; set; }
        public Enemy Enemy10 { get; set; }
        public Enemy Enemy11 { get; set; }
        public Enemy Enemy12 { get; set; }
        public Enemy Enemy13 { get; set; }
        public Enemy Enemy14 { get; set; }
        public Enemy Enemy15 { get; set; }
        public Enemy Enemy16 { get; set; }
        public Enemy Enemy17 { get; set; }
        public Enemy Enemy18 { get; set; }
        public Enemy Enemy19 { get; set; }
        public Enemy Enemy20 { get; set; }

        private List<Enemy> EnemyList { get; set; }

        private List<Enemy> ActiveEnemies
        {
            get
            {
                return
                    EnemyList.Where(x => x.IsActive).ToList();
            }
        }

        private void InitializeEnemies()
        {
            Enemy1 = new Enemy();
            Enemy2 = new Enemy();
            Enemy3 = new Enemy();
            Enemy4 = new Enemy();
            Enemy5 = new Enemy();
            Enemy6 = new Enemy();
            Enemy7 = new Enemy();
            Enemy8 = new Enemy();
            Enemy8 = new Enemy();
            Enemy9 = new Enemy();
            Enemy10 = new Enemy();
            Enemy11 = new Enemy();
            Enemy12 = new Enemy();
            Enemy13 = new Enemy();
            Enemy14 = new Enemy();
            Enemy15 = new Enemy();
            Enemy16 = new Enemy();
            Enemy17 = new Enemy();
            Enemy18 = new Enemy();
            Enemy19 = new Enemy();
            Enemy20 = new Enemy();

            EnemyList =
                new List<Enemy> { Enemy1, Enemy2, Enemy3, Enemy4, Enemy5, 
                    Enemy6, Enemy7, Enemy8, Enemy9, Enemy10, Enemy11, Enemy12,
                    Enemy13, Enemy14, Enemy15, Enemy16, Enemy17, Enemy18, Enemy19, Enemy20};

            PlaceEnemies(0, GameConfigurations.EnemyHeight);
        }

        #endregion

        #region Enemy movement

        private async void StartEnemyMove()
        {
            await Task.Run(() =>
            {
                while (IsActiveGameMode)
                {
                    if (!ActiveEnemies.Any()) FinishGame(true);

                    if (IsTimeToChangeBackGroundTheme())
                    {
                        _dispatcherFunction(BackgroundManager.UpdateTheme);
                    }

                    var coordinates =
                        IsTimeToDescend() ?
                            GetDescendingCoordinates() : GetMoveCoordinates();

                    _dispatcherFunction(() => PlaceEnemies(coordinates[0], coordinates[1]));
                    _enemyConfigurations.MoveNumber++;

                    if (HasEnemyReachedPlayer())
                    {
                        FinishGame(false);
                        return;
                    }

                    Thread.Sleep(_enemyConfigurations.MoveSpeed);
                }
            });
        }

        private void PlaceEnemies(Double left, Double top)
        {
            Enemy1.Move(Enemy1.Left + left, Enemy1.Top + top);
            Enemy2.Move(Enemy1.Left + GameConfigurations.EnemyWidth * 1.5, Enemy1.Top);
            Enemy3.Move(Enemy2.Left + GameConfigurations.EnemyWidth * 1.5, Enemy1.Top);
            Enemy4.Move(Enemy3.Left + GameConfigurations.EnemyWidth * 1.5, Enemy1.Top);

            Enemy5.Move(Enemy1.Left * 1.5, Enemy1.Top + GameConfigurations.EnemyHeight * 1.5);
            Enemy6.Move(Enemy5.Left + GameConfigurations.EnemyWidth * 1.5, Enemy5.Top);
            Enemy7.Move(Enemy6.Left + GameConfigurations.EnemyWidth * 1.5, Enemy5.Top);
            Enemy8.Move(Enemy7.Left + GameConfigurations.EnemyWidth * 1.5, Enemy5.Top);

            Enemy9.Move(Enemy1.Left * 1.25, Enemy5.Top + GameConfigurations.EnemyHeight * 1.5);
            Enemy10.Move(Enemy9.Left + GameConfigurations.EnemyWidth * 1.5, Enemy9.Top);
            Enemy11.Move(Enemy10.Left + GameConfigurations.EnemyWidth * 1.5, Enemy9.Top);
            Enemy12.Move(Enemy11.Left + GameConfigurations.EnemyWidth * 1.5, Enemy9.Top);

            Enemy13.Move(Enemy1.Left * 1.5, Enemy10.Top + GameConfigurations.EnemyHeight * 1.5);
            Enemy14.Move(Enemy13.Left + GameConfigurations.EnemyWidth * 1.5, Enemy13.Top);
            Enemy15.Move(Enemy14.Left + GameConfigurations.EnemyWidth * 1.5, Enemy13.Top);
            Enemy16.Move(Enemy15.Left + GameConfigurations.EnemyWidth * 1.5, Enemy13.Top);

            Enemy17.Move(Enemy1.Left, Enemy13.Top + GameConfigurations.EnemyHeight * 1.5);
            Enemy18.Move(Enemy17.Left + GameConfigurations.EnemyWidth * 1.5, Enemy17.Top);
            Enemy19.Move(Enemy18.Left + GameConfigurations.EnemyWidth * 1.5, Enemy17.Top);
            Enemy20.Move(Enemy19.Left + GameConfigurations.EnemyWidth * 1.5, Enemy17.Top);
        }

        private Boolean IsTimeToDescend()
        {
            return
                (_enemyConfigurations.MoveNumber >= _enemyConfigurations.DescendTime) &&
                (_enemyConfigurations.MoveNumber % _enemyConfigurations.DescendTime) == 0;
        }

        private Boolean IsTimeToChangeBackGroundTheme()
        {
            return
                (_enemyConfigurations.MoveNumber >= GameConfigurations.MovesNumberToChangeTheme) &&
                (_enemyConfigurations.MoveNumber % GameConfigurations.MovesNumberToChangeTheme) == 0;
        }

        private Double[] GetMoveCoordinates()
        {
            var moveDestination = GetMoveDestination();

            switch (moveDestination)
            {
                case (MoveDestination.Left):
                    return new[] { GameConfigurations.EnemyWidth / 2 * (-1), 0 };
                default:
                    return new[] { GameConfigurations.EnemyWidth / 2, 0 };
            }
        }

        private Double[] GetDescendingCoordinates()
        {
            return new[] { 0, GameConfigurations.EnemyHeight };
        }

        private MoveDestination GetMoveDestination()
        {
            var random = RandomProvider.Random;
            var moveDestination = (MoveDestination)random.Next(0, 2);

            try
            {
                if (IsAtTopRightCorner()) return MoveDestination.Left;
                if (IsAtTopLeftCorner()) return MoveDestination.Right;
            }
            catch (Exception)
            {
            }

            return moveDestination;
        }

        private Boolean IsAtTopRightCorner()
        {
            var maxLeft = ActiveEnemies.Max(x => x.Left);

            return
                maxLeft >=
                    (ScreenConfigurations.RightCorner + GameConfigurations.EnemyWidth);
        }

        private Boolean IsAtTopLeftCorner()
        {
            var minLeft = ActiveEnemies.Min(x => x.Left);

            return
                minLeft <=
                (ScreenConfigurations.LeftCorner - GameConfigurations.EnemyWidth * 0.5);
        }

        private Boolean HasEnemyReachedPlayer()
        {
            return
                ActiveEnemies.Any(x =>
                    x.Top >=
                    (ScreenConfigurations.PlayerTop - GameConfigurations.EnemyHeight));
        }

        #endregion

        #region Enemy fire

        private async void StartEnemyAttack()
        {
            await Task.Run(() =>
            {
                while (IsActiveGameMode)
                {
                    EnemyFire();

                    Thread.Sleep(_enemyConfigurations.AttackSpeed);
                }
            });
        }

        private async void EnemyFire()
        {
            var bullet = GetInactiveBullet(EnemyBulletList);
            if (bullet == null) return;

            await Task.Run(() =>
            {
                _dispatcherFunction(() => SetEnemyBullet(bullet));

                while (bullet.Top < ScreenConfigurations.BottomCorner)
                {
                    _dispatcherFunction(bullet.Move);

                    var hitPlayer = IsPlayerHit(bullet);

                    if (IsPlayerHit(bullet))
                    {
                        _dispatcherFunction(() =>
                        {
                            bullet.Reset();
                            Player.IsActive = false;
                        });

                        FinishGame(false);
                        return;
                    }

                    Thread.Sleep(GameConfigurations.BulletSpeed);
                }

                _dispatcherFunction(bullet.Reset);
            });
        }

        private Boolean IsPlayerHit(BulletBase bullet)
        {
            return
                (bullet.Left >= Player.Left &&
                    bullet.Left <= Player.Left + GameConfigurations.PlayerWidth)
                &&
                (bullet.Top >= Player.Top &&
                    bullet.Top <= Player.Top + GameConfigurations.PlayerHeight);
        }

        private void SetEnemyBullet(BulletBase bullet)
        {
            var random = RandomProvider.Random;
            var enemyToFirePosition = random.Next(0, ActiveEnemies.Count);
            Enemy enemyToFire;

            try
            {
                enemyToFire =
                    ActiveEnemies.Skip(enemyToFirePosition - 1).First();
            }
            catch (Exception)
            {
                enemyToFire = EnemyList.First();
            }

            bullet.Left = enemyToFire.Left + GameConfigurations.EnemyWidth * 0.5;
            bullet.Top = enemyToFire.Top + GameConfigurations.EnemyHeight * 0.75;
        }

        #endregion
    }
}
