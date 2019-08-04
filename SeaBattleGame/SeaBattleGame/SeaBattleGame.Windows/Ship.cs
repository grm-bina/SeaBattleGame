using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace SeaBattleGame
{
    class Ship
    {
        private Rectangle _shipBorder;
        private Cell[] _decks;
        private Cell[] _space;
        private int _numOfDecks;
        private char _direction;     // 'V' is vertical  /  'H' is horisontal

        Random rnd = new Random();

        // Constructor
        public Ship(int numOfDecks, Canvas playerCanvas, Board playerBoard, int shipIndex)
        {
            _numOfDecks = numOfDecks;
            // Check place for build new ship
            bool succesful = false;
            while (!succesful)
            {
                int firstDeckIndexY = rnd.Next(playerBoard.CellsInSide);
                int firstDeckIndexX = rnd.Next(playerBoard.CellsInSide);
                succesful = playerBoard.IsCanPlaceShip(_numOfDecks, firstDeckIndexY, firstDeckIndexX, out _decks, out _direction);
            }
            // Place ship in board
            for (int i = 0; i < _decks.Length; i++)
            {
                _decks[i].IsDeck = true;
                _decks[i].ShipIndex = shipIndex;
            }//now ship is exist but invisible

            // Build rectangle for ship's visibility
            int size = playerBoard.SizeOfCells;
            int lenght = size * numOfDecks;
            _shipBorder = new Rectangle();
            if (_direction == 'V')
            {
                _shipBorder.Width = size;
                _shipBorder.Height = lenght;
            }
            else if (_direction == 'H')
            {
                _shipBorder.Width = lenght;
                _shipBorder.Height = size;
            }
            _shipBorder.Stroke = new SolidColorBrush(Colors.Blue);
            _shipBorder.StrokeThickness = 2;
            Canvas.SetLeft(_shipBorder, _decks[0].IndexX * size);
            Canvas.SetTop(_shipBorder, _decks[0].IndexY * size);
            playerCanvas.Children.Add(_shipBorder);//now ship is visible

            // Build space around the ship
            playerBoard.MakeSpace(_decks, out _space);
            
        }

        #region Property
        // Property is ship Alive
        public bool IsAlive
        {
            get
            {
                for (int i = 0; i < _decks.Length; i++)
                {
                    if (!_decks[i].IsShot)
                        return true;
                }
                return false;
            }
        }

        // Property Visibility ship's border On/Off
        public bool ShowBorder
        {
            set
            {
                if (value)
                    _shipBorder.Visibility = Visibility.Visible;
                else
                    _shipBorder.Visibility = Visibility.Collapsed;
            }
        }
        #endregion]
        
        // Function Shoot all space (use when ship is NOT alive) -show and disable space around ship
        public void SpaceToShot()
        {
            for (int i = 0; i < _space.Length; i++)
            {
                if(!_space[i].IsShot)
                    _space[i].IsShot = true;
            }
        }


    }
}
