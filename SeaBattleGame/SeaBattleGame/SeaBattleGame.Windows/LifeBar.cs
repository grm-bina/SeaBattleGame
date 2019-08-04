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
    class LifeBar
    {
        private Rectangle _border;
        private Rectangle _fill;
        private Player _player;

        // constructor
        public LifeBar(Canvas canvas, int positionY, int positionX, Player player)
        {
            _fill = new Rectangle();
            _fill.Width = 300;
            _fill.Height = 15;
            _fill.Fill = new SolidColorBrush(Colors.Blue);
            Canvas.SetLeft(_fill, positionX);
            Canvas.SetTop(_fill, positionY);
            canvas.Children.Add(_fill);

            _border = new Rectangle();
            _border.Width = 300;
            _border.Height = 15;
            _border.Stroke = _fill.Fill;
            _border.StrokeThickness = 1;
            Canvas.SetLeft(_border, positionX);
            Canvas.SetTop(_border, positionY);
            canvas.Children.Add(_border);

            _player = player;
        }

        // Property Visibility
        public bool IsVisible
        {
            set
            {
                if (value)
                {
                    _border.Visibility = Visibility.Visible;
                    _fill.Visibility = Visibility.Visible;
                }
                else
                {
                    _border.Visibility = Visibility.Collapsed;
                    _fill.Visibility = Visibility.Collapsed;
                }
            }
        }

        // Function refresh LifeBar when ship destroyed
        public void Refresh()
        {
            if (_player.ShipsNum < 2)
            {
                _fill.Fill = new SolidColorBrush(Colors.Red);
                _border.Stroke = _fill.Fill;
                if(_player.ShipsNum == 0)
                    _fill.Width = 1;
                else
                    _fill.Width = (_border.Width / 10) * _player.ShipsNum;
            }
            else
            {
                _fill.Fill = new SolidColorBrush(Colors.Blue);
                _border.Stroke = _fill.Fill;
                _fill.Width = (_border.Width / 10) * _player.ShipsNum;
            }
        }

        // Function refresh reference to board (for new game & replace ships)
        public void GetBoard (Player player)
        {
            _player = player;
        }

    }
}
