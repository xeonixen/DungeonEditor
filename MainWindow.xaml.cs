using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Microsoft.Win32;

namespace DungeonEditorV2
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly DispatcherTimer _timer = new DispatcherTimer();
        private Point _mouseDragOffset;
        private bool _moveCanvas;
        private int _roomTemplateIndex;
        private bool _updatePosition;
        public ScaleTransform GlobalScaleTransform = new ScaleTransform(0.2d, 0.2d);
        public Level Level;

        public MainWindow()
        {
            InitializeComponent();
            Level = new Level();
            MouseDown += MouseDownEventHandler;
            MouseUp += MouseDownEventHandler;
            MouseWheel += MouseWheelEventHandler;
            KeyDown += KeyHandler;
            InitTimer();
            AddRoomTemplates();
            // InitGrid();
        }

        //private void InitGrid()
        //{
        //    var grid = new List<Shape>();
        //    for (var i = (int) (Level.TileSize*GlobalScaleTransform.ScaleX);
        //        i < Level.Size.Height;
        //        i += (int) (Level.TileSize*GlobalScaleTransform.ScaleX))
        //    {
        //        grid.Add(new Line
        //        {
        //            X1 = 0,
        //            Y1 = i,
        //            X2 = Level.Size.Width,
        //            Y2 = i,
        //            Stroke = Brushes.Gray,
        //            StrokeThickness = 1
        //        });
        //    }
        //    for (var i = (int) (Level.TileSize*GlobalScaleTransform.ScaleX);
        //        i < Level.Size.Width;
        //        i += (int) (Level.TileSize*GlobalScaleTransform.ScaleX))
        //    {
        //        grid.Add(new Line
        //        {
        //            X1 = i,
        //            Y1 = 0,
        //            X2 = i,
        //            Y2 = Level.Size.Height,
        //            Stroke = Brushes.Gray,
        //            StrokeThickness = 1
        //        });
        //    }
        //    grid.ForEach(a => BaseCanvas.Children.Add(a));
        //    BaseCanvas.Width = Level.Size.Width;
        //    BaseCanvas.Height = Level.Size.Height;
        //}

        private void AddRoomTemplates()
        {
            for (var i = 1; i < 7; i++)
                Level.AvailableRoomTemplates.Add($"pack://application:,,,/Images/Dungion{i}.png");
        }

        

        private void KeyHandler(object sender, KeyEventArgs e)
        {
            var mousePosition = Mouse.GetPosition(BaseCanvas);
            switch (e.Key)
            {
                case Key.N:
                    Level.AddRoom(new Room(GlobalScaleTransform, mousePosition)
                    {
                        Image =
                            new Image
                            {
                                Source =
                                    new BitmapImage(new Uri(Level.AvailableRoomTemplates[_roomTemplateIndex],
                                        UriKind.Absolute))
                            }
                    });
                    BaseCanvas.Children.Add(Level.CurrentRoom.Image);
                    break;
                case Key.P:
                    MessageBox.Show(Level.GetRoomData());
                    break;
                case Key.O:
                    
                    break;
            }
        }

        private void InitTimer()
        {
            _timer.Interval = TimeSpan.FromMilliseconds(20);
            _timer.Tick += _timer_Tick;
            _timer.Start();
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            var mpos = Mouse.GetPosition(BaseCanvas);
            if (_updatePosition)
            {
                Level.CurrentRoom.Position = new Point(mpos.X - _mouseDragOffset.X, mpos.Y - _mouseDragOffset.Y);
            }
            if (_moveCanvas)
            {
            }
        }

        private void MouseWheelEventHandler(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta < 0)
            {
                _roomTemplateIndex--;
            }
            if (e.Delta > 0)
            {
                _roomTemplateIndex++;
            }
            if (_roomTemplateIndex > Level.AvailableRoomTemplates.Count - 1)
                _roomTemplateIndex = Level.AvailableRoomTemplates.Count - 1;
            if (_roomTemplateIndex < 0) _roomTemplateIndex = 0;
            if (Level.CurrentRoom != null)
                Level.CurrentRoom.Image.Source =
                    new BitmapImage(new Uri(Level.AvailableRoomTemplates[_roomTemplateIndex], UriKind.Absolute));
        }

        private void MouseDownEventHandler(object sender, MouseButtonEventArgs e)
        {
            var mousePosition = e.GetPosition(BaseCanvas);
            switch (e.ChangedButton)
            {
                case MouseButton.Left:
                    switch (e.ButtonState)
                    {
                        case MouseButtonState.Pressed:
                            var over = Mouse.DirectlyOver as Image;
                            if (over != null)
                                Level.SetCurrentRoom(over);
                            if (Level.CurrentRoom != null)
                                _mouseDragOffset = new Point(mousePosition.X - Level.CurrentRoom.Position.X,
                                    mousePosition.Y - Level.CurrentRoom.Position.Y);
                            if (Level.CurrentRoom != null) _updatePosition = true;
                            break;
                        case MouseButtonState.Released:
                            _updatePosition = false;
                            break;
                    }
                    break;
                case MouseButton.Right:
                    switch (e.ButtonState)
                    {
                        case MouseButtonState.Pressed:
                            _moveCanvas = true;
                            _mouseDragOffset = new Point(mousePosition.X - Level.CurrentRoom.Position.X,
                                mousePosition.Y - Level.CurrentRoom.Position.Y);
                            break;
                        case MouseButtonState.Released:
                            _moveCanvas = false;
                            break;
                    }
                    break;
            }
        }

        private void MenuOpenRoomClick(object sender, RoutedEventArgs e)
        {
            var fdg = new OpenFileDialog();
            fdg.ShowDialog();
            if (!string.IsNullOrWhiteSpace(fdg.FileName))
            {
               //TODO  add to room list.
            }
        }

        private void MenuQuitClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MenuRoomEditorClick(object sender, RoutedEventArgs e)
        {
            var re = new RoomEditor();
            re.Show();
        }
    }
}