using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DungeonEditorV2
{
    public class Room
    {
        private Image _image;
        private Point _position;
        private bool _selected;
        public Tile[] Tiles { get; set; }
        public ScaleTransform Scale { get; }
        public RotateTransform Rotate { get; } = new RotateTransform();

        public Room(ScaleTransform globalScale, double x, double y):this(globalScale,new Point(x, y)) { }
        public Room(Point position) : this(new ScaleTransform(1, 1), position) { }
        public Room(ScaleTransform globalScale, Point position)
        {
            Scale = globalScale;
            Position = position;
            Image=new Image();
        }

        public RoutedEventHandler MenuItemClickHandler { get; set; }

        public MenuItem[] AvailableActions
        {
            get
            {
                var items = new List<MenuItem>();
                var item = new MenuItem {Header = "Delete"};
                item.Click += MenuItemClickHandler;
                items.Add(item);
                return items.ToArray();
            }
        }


        public Image Image
        {
            get { return _image; }
            set
            {
                _image = value;
                var tg = new TransformGroup();
                tg.Children.Add(Scale);
                tg.Children.Add(Rotate);
                _image.RenderTransform = tg;
                _image.SetValue(Canvas.LeftProperty, Position.X);
                _image.SetValue(Canvas.TopProperty, Position.Y);
            }
        }

       
        public bool Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                Image.Opacity = _selected ? .6d : 1d;
            }
        }

        public Point Position
        {
            get { return _position; }
            set
            {
                _position = value;
                Image?.SetValue(Canvas.LeftProperty, Position.X);
                Image?.SetValue(Canvas.TopProperty, Position.Y);
            }
        }
    }
}