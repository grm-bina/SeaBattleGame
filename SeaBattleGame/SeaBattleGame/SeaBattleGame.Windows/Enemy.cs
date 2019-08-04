using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace SeaBattleGame
{
    class Enemy : Player
    {
        Random rnd = new Random();
        private GameManager _manager; // referense to manager for initiate User's Move in event
        // Data members for key-events
        private int _tempPositionY;
        private int _tempPositionX;
        // Canvas for pointer events
        private Canvas _canvas;

        // Constructor
        public Enemy(Canvas playerCanvas, GameManager manager):base(playerCanvas)
        {
            _manager = manager;
            _canvas = playerCanvas;

            // Make all ships unvisible
            for (int i = 0; i < _playerNavy.Length; i++)
            {
                _playerNavy[i].ShowBorder = false;
            }

            _tempPositionY = rnd.Next(10);
            _tempPositionX = rnd.Next(10);


        }

        // Override for Player-function
        protected override void DestroyShip(Ship destroyingShip)
        {
            destroyingShip.ShowBorder = true; // Make 1 ship visible when is not alive
            base.DestroyShip(destroyingShip);
        }

        #region Events & Functions for events

        // 2 Functions for focus
        public void CellInFocus()
        {
            CellsBoard[_tempPositionY, _tempPositionX].ShowAimIcon();
        }
        public void FocusLost()
        {
            CellsBoard[_tempPositionY, _tempPositionX].RemoveAimIcon();
        }

        // Function Events On
        public void AddEvents()
        {
            //keyboard
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
            CellsBoard[_tempPositionY, _tempPositionX].ShowAimIcon();
            //pointer
            _canvas.PointerPressed += _canvas_PointerPressed;
            _canvas.PointerMoved += _canvas_PointerMoved;
        }


        // Function Events Off
        public void RemoveEvents()
        {
            //keyboard
            Window.Current.CoreWindow.KeyDown -= CoreWindow_KeyDown;
            //pointer
            _canvas.PointerPressed -= _canvas_PointerPressed;
            _canvas.PointerMoved -= _canvas_PointerMoved;
        }

        // Keyboard event
        private void CoreWindow_KeyDown(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            if (args.VirtualKey == Windows.System.VirtualKey.Enter)
            {
                _manager.Move(_tempPositionY, _tempPositionX, this);
            }
            else if (args.VirtualKey == Windows.System.VirtualKey.Up)
            {
                if (_tempPositionY - 1 >= 0)
                {
                    FocusLost();
                    _tempPositionY--;
                    CellInFocus();
                }
            }
            else if (args.VirtualKey == Windows.System.VirtualKey.Down)
            {
                if (_tempPositionY + 1 < 10)
                {
                    FocusLost();
                    _tempPositionY++;
                    CellInFocus();
                }

            }
            else if (args.VirtualKey == Windows.System.VirtualKey.Right)
            {
                if (_tempPositionX + 1 < 10)
                {
                    FocusLost();
                    _tempPositionX++;
                    CellInFocus();
                }

            }
            else if (args.VirtualKey == Windows.System.VirtualKey.Left)
            {
                if (_tempPositionX - 1 >= 0)
                {
                    FocusLost();
                    _tempPositionX--;
                    CellInFocus();
                }

            }
        }


        // Pointer event - Pressed
        private void _canvas_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            _tempPositionY = (int)(e.GetCurrentPoint(_canvas).Position.Y / SizeOfCells);
            _tempPositionX = (int)((e.GetCurrentPoint(_canvas).Position.X)/SizeOfCells);
            _manager.Move(_tempPositionY, _tempPositionX, this);
        }

        // Pointer Move
        private void _canvas_PointerMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            int tempY = (int)(e.GetCurrentPoint(_canvas).Position.Y / SizeOfCells);
            int tempX = (int)((e.GetCurrentPoint(_canvas).Position.X) / SizeOfCells);
            if ((_tempPositionY != tempY || _tempPositionX != tempX))
            {
                FocusLost();
                _tempPositionY = tempY;
                _tempPositionX = tempX;
                CellInFocus();
            }
        }

        #endregion




    }
}
