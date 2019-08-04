using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

namespace SeaBattleGame
{
    // class of cells for gameboard
    class Cell
    {

        private Rectangle _cell;
        private ImageBrush _shotIcon;   // fill rectangle when shot
        private ImageBrush _aimIcon;    // fill rectangle when pointer over
        private Ellipse _aimDisable; // show when pointer over but cell is shot
        private bool _isShot;
        private bool _isSpace; // if the cell is the space around ship (forbidden to place deck)
        private bool _isDeck; // if the cell is ship's deck
        private int _shipIndex;  // if it's deck - position in navy-array
        private int _indexY;   // position in board-array
        private int _indexX;   // position in board-array


        // Constructor
        public Cell(int size, double positionX, double positionY, int indexY, int indexX, Canvas playerCanvas)
        {
            _indexY = indexY;
            _indexX = indexX;

            _cell = new Rectangle();
            _cell.Width = size;
            _cell.Height = size;
            _cell.Stroke = new SolidColorBrush(Colors.Blue);
            _cell.StrokeThickness = 0.2;
            Canvas.SetLeft(_cell, positionX);
            Canvas.SetTop(_cell, positionY);
            playerCanvas.Children.Add(_cell);

            // pointer area smaller then cell (board manage it and add events)
            _aimDisable = new Ellipse();
            _aimDisable.Width = size / 1.5;
            _aimDisable.Height = size / 1.5;
            Canvas.SetLeft(_aimDisable, (positionX + size/2 - (_aimDisable.Width / 2)));
            Canvas.SetTop(_aimDisable, (positionY + size/2 - (_aimDisable.Height / 2)));
            playerCanvas.Children.Add(_aimDisable);

            _aimIcon = new ImageBrush();
            _aimIcon.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/Images/aim.png"));

            _shotIcon = new ImageBrush();
            _shotIcon.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/Images/miss.png"));

            _shipIndex = -1; // change index to positive when convert to deck
        }

        #region Property

        // Property Position Y in array(board)
        public int IndexY
        {
            get
            {
                return _indexY;
            }
        }

        // Property Position X in array(board)
        public int IndexX
        {
            get
            {
                return _indexX;
            }
        }


        // Property is cell is ship's deck
        public bool IsDeck
        {
            get
            {
                return _isDeck;
            }
            set  // convert cell to deck
            {
                if (value && !_isSpace)
                {
                    _isDeck = true;
                    _shotIcon.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/Images/hit.png"));
                }
            }
        }

        // Property Ship Index
        public int ShipIndex
        {
            get
            {
                return _shipIndex;
            }
            set
            {
                _shipIndex = value;
            }
        }


        // Property cell is shot
        public bool IsShot
        {
            get
            {
                return _isShot;
            }
            set
            {
                if (value)
                {
                    _isShot = true;
                    _cell.Fill = _shotIcon;
                }
            }
        }

        // Property cell is space around ship
        public bool IsSpace
        {
            get
            {
                return _isSpace;
            }
            set
            {
                if (value && !_isDeck)
                    _isSpace = true;
            }
        }

        #endregion

        #region Functions
        // Function Feedback after Move (shot cell)
        public bool Fire()
        {
            if (!_isShot)
            {
                _cell.Fill = _shotIcon;
                _isShot = true;
            }
            return _isDeck;
        }

        // 2 Function for events entered/exed - change cell-image to aim
        public void RemoveAimIcon()     //exed
        {
            if (!IsShot)
                _cell.Fill = null;
            else
            {
                _aimDisable.Stroke = null;
                _aimDisable.StrokeThickness = 0;
            }
        }
        public void ShowAimIcon()   //entered
        {
            if (!IsShot)
                _cell.Fill = _aimIcon;
            else
            {
                _aimDisable.Stroke = new SolidColorBrush(Colors.Red);
                _aimDisable.StrokeThickness = 0.6;
            }
        }
        #endregion

    }
}
