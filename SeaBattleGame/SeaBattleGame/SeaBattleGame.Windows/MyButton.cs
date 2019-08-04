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
    class MyButton
    {
        private Rectangle _border;
        public TextBlock Content { get; set; }

        // Constructor
        public MyButton(Canvas canvas, int positionY, int positionX, TextBlock template)
        {
            _border = new Rectangle();
            _border.Width = 120;
            _border.Height = 30;
            _border.Stroke= new SolidColorBrush(Colors.Blue);
            _border.StrokeThickness = 1.5;
            Canvas.SetLeft(_border, positionX);
            Canvas.SetTop(_border, positionY);
            canvas.Children.Add(_border);

            Content = new TextBlock();
            Content.Width = _border.Width;
            Content.Height = _border.Height;
            Content.Foreground = new SolidColorBrush(Colors.Blue);
            Content.FontSize = 18;
            Content.FontFamily = template.FontFamily;
            Content.TextAlignment = template.TextAlignment;
            Canvas.SetLeft(Content, positionX);
            Canvas.SetTop(Content, positionY);
            canvas.Children.Add(Content);
        }

        // Property Visibility
        public bool IsVisible
        {
            set
            {
                if (value)
                {
                    _border.Visibility = Visibility.Visible;
                    Content.Visibility = Visibility.Visible;
                }
                else
                {
                    _border.Visibility = Visibility.Collapsed;
                    Content.Visibility = Visibility.Collapsed;
                }
            }
        }

        // function switch color for winking
        public async void SwitchColor()
        {
            await Task.Delay(500);
            _border.Stroke = new SolidColorBrush(Colors.Red);
            await Task.Delay(500);
            _border.Stroke = new SolidColorBrush(Colors.Blue);
        }



    }
}
