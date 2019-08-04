using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace SeaBattleGame
{
    class GameManager
    {
        private AutoShooter _autoShooter; // enemy's "brain" for move

        private Player _user;
        private Enemy _comp;
        private Canvas _userCanvas;
        private Canvas _compCanvas;
        private Random rnd = new Random();

        // bars for visualizate num of player's ships-alive
        private LifeBar _lifeUser;
        private LifeBar _lifeComp;

        private MyButton _newGame;
        private MyButton _exit;
        private MyButton _replaceShips;
        private MyButton _startGame;

        private TextBlock _message;

        DispatcherTimer _buttonsWinker;

        // Constructor
        public GameManager(Canvas userCanvas, Canvas enemyCanvas, Canvas backgroundCanvas, TextBlock message)
        {
            _message = message;
            _message.Text = "Press 'Start' to play...";

            _userCanvas = userCanvas;
            _compCanvas = enemyCanvas;
            _user = new Player(_userCanvas);
            _comp = new Enemy(_compCanvas, this);
            _autoShooter = new AutoShooter(this, _user);

            // Build Buttons
            int margin = 15;
            _newGame = new MyButton(backgroundCanvas, 15, 15, message);
            _newGame.Content.Text = "New game";
            _newGame.Content.PointerEntered += Content_PointerEntered;
            _newGame.Content.PointerExited += Content_PointerExited;
            _newGame.Content.Tapped += NewGame_Tapped;

            _exit = new MyButton(backgroundCanvas, margin, (int)(backgroundCanvas.Width - margin - _newGame.Content.Width), message);
            _exit.Content.Text = "Exit";
            _exit.Content.PointerEntered += Content_PointerEntered;
            _exit.Content.PointerExited += Content_PointerExited;
            _exit.Content.Tapped += Exit_Tapped;

            _replaceShips = new MyButton(backgroundCanvas, (int)(backgroundCanvas.Height - margin*5) , (margin * 9), message);
            _replaceShips.Content.Text = "Replace";
            _replaceShips.Content.PointerEntered += Content_PointerEntered;
            _replaceShips.Content.PointerExited += Content_PointerExited;
            _replaceShips.Content.Tapped += NewGame_Tapped;

            _startGame = new MyButton(backgroundCanvas, (int)(backgroundCanvas.Height - margin * 5), (int)(margin * 11 + _replaceShips.Content.Width), message);
            _startGame.Content.Text = "Start";
            _startGame.Content.PointerEntered += Content_PointerEntered;
            _startGame.Content.PointerExited += Content_PointerExited;
            _startGame.Content.Tapped += Start_Tapped;

            _buttonsWinker = new DispatcherTimer();
            _buttonsWinker.Interval = new TimeSpan(0, 0, 1);
            _buttonsWinker.Tick += _buttonsWinker_Tick;
            _buttonsWinker.Start();

            // Build LifeBars
            _lifeUser = new LifeBar(backgroundCanvas, (int)(backgroundCanvas.Height - margin * 5), (margin * 8), _user);
            _lifeUser.IsVisible = false;
            _lifeComp = new LifeBar(backgroundCanvas, (int)(backgroundCanvas.Height - margin * 5), (int)(backgroundCanvas.Width - (margin * 28)), _comp);
            _lifeComp.IsVisible = false;


        }

        #region Button's Functions
        // Function New Game ( use for new-game-button & replace-ships-button)
        public void NewGame()
        {
            _comp.RemoveEvents();

            _userCanvas.Children.Clear();
            _compCanvas.Children.Clear();
            _lifeUser.GetBoard(null);
            _lifeComp.GetBoard(null);

            _user = new Player(_userCanvas);
            _comp = new Enemy(_compCanvas, this);
            _autoShooter = new AutoShooter(this, _user);

            _lifeUser.GetBoard(_user);
            _lifeUser.Refresh();
            _lifeUser.IsVisible=false;
            _lifeComp.GetBoard(_comp);
            _lifeComp.Refresh();
            _lifeComp.IsVisible = false;

            _buttonsWinker.Stop();
            _message.Text = "Press 'Start' to play...";
            _buttonsWinker.Start();

            _replaceShips.IsVisible = true;
            _startGame.IsVisible = true;
        }

        // Function Game Start
        public void Start()
        {
            _replaceShips.IsVisible = false;
            _startGame.IsVisible = false;
            _lifeUser.IsVisible = true;
            _lifeComp.IsVisible = true;
            _message.Text = "Your move...";

            _buttonsWinker.Stop();

            // Events On
            _comp.AddEvents();

        }

        // Function Game Stop
        private void Stop()
        {
            if (_user.ShipsNum > _comp.ShipsNum)
            {
                _message.Text = "Win!";
                _lifeComp.Refresh();
            }
            else
            {
                _message.Text = "Lose :(";
                _lifeUser.Refresh();
            }

            _buttonsWinker.Start();

            // board events Off
            _comp.RemoveEvents();
            
        }
        #endregion

        // Function Move
        public async void Move(int indexY, int indexX, Player boardUnderFire)
        {
            string feedback =  boardUnderFire.UnderFire(indexY, indexX);
            switch (feedback)
            {
                case "again":
                    if (boardUnderFire == _user)
                    {
                        _autoShooter.EnemyMove(feedback);
                    }
                    break;
                case "ship destroyed":
                    if (boardUnderFire == _user)
                    {
                        _lifeUser.Refresh();
                        await Task.Delay(500);
                        _autoShooter.EnemyMove(feedback);
                        await Task.Delay(500);
                    }
                    else
                    {
                        _lifeComp.Refresh();
                        _comp.CellInFocus();
                    }
                    break;
                case "hit":
                    if (boardUnderFire == _user)
                    {
                        await Task.Delay(500);
                        _autoShooter.EnemyMove(feedback);
                        await Task.Delay(500);
                    }
                    else
                    {
                        _comp.CellInFocus();
                    }
                    break;
                case "navy destroyed":
                    Stop();
                    break;
                case "miss":
                    if (boardUnderFire == _user)
                    {
                        await Task.Delay(500);
                        _message.Text = "Your move...";
                        _comp.AddEvents();
                    }
                    else
                    {
                        _comp.RemoveEvents();
                        _message.Text = "Wait...";
                        await Task.Delay(500);
                        _autoShooter.EnemyMove(feedback);
                        await Task.Delay(500);
                    }
                    break;
            }
        }

        #region Events for Buttons

        // winking buttons new-game (when game is finished) & button start
        // dispatcher timer
        private void _buttonsWinker_Tick(object sender, object e)
        {
            switch (_message.Text)
            {
                case "Press 'Start' to play...":
                    _startGame.SwitchColor();
                    break;
                case "Win!":
                case "Lose :(":
                    _newGame.SwitchColor();
                    break;
            }
        }

        // 2 events for all buttons (animation for pointer entred/exed)
        private void Content_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            ((TextBlock)sender).Foreground = new SolidColorBrush(Colors.Red);
        }
        private void Content_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            ((TextBlock)sender).Foreground = new SolidColorBrush(Colors.Blue);
        }

        // Click Start Button
        private void Start_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            Start();
        }

        // Click Exit Button
        private async void Exit_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            _message.Text = "Goodbye!";
            await Task.Delay(1000);
            Application.Current.Exit();
        }

        // Click NewGame Button or Replace ships button
        private void NewGame_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            NewGame();
        }

        #endregion
    }
}
