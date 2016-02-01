using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.Win32;

namespace DungeonEditorV2
{
    /// <summary>
    ///     Interaction logic for RoomEditor.xaml
    /// </summary>
    public partial class RoomEditor
    {
        private Room _room = new Room(new Point());
        private TileTypes _selectedTileType;
        //private Tile[] _roomTiles;
        private readonly int _tilesize = 43;
        private bool _lMouseButtonDown ;
        DispatcherTimer _timer = new DispatcherTimer();

        public RoomEditor()
        {
            InitializeComponent();
            MouseLeftButtonDown += RoomEditor_MouseLeftButtonDown;
            MouseRightButtonDown += RoomEditor_MouseRightButtonDown;
            MouseLeftButtonUp += RoomEditor_MouseLeftButtonUp;
            _timer.Interval=TimeSpan.FromMilliseconds(20);
            _timer.Tick += _timer_Tick;
            _timer.Start();
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            var mpos = Mouse.GetPosition(BaseCanvas);
            if (_lMouseButtonDown)
            {
                var index =
                (int)
                    (Math.Floor(mpos.Y / _tilesize) * Math.Ceiling(_room.Image.Source.Width / _tilesize) +
                     Math.Floor(mpos.X / _tilesize));
                if (index >= 0 && index < _room.Tiles.Length) _room.Tiles[index].TileType = _selectedTileType;
            }
        }

        private void RoomEditor_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _lMouseButtonDown = false;
        }

        private void RoomEditor_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var cm = new ContextMenu();
            foreach (var name in Enum.GetNames(typeof (TileTypes)))
            {
                cm.Items.Add(new MenuItem {Header = name});
            }

            foreach (var item in cm.Items)
            {
                var mitem = item as MenuItem;
                if (mitem != null) mitem.Click += TileContextMenuClick;
            }
            BaseCanvas.ContextMenu = cm;
        }

        private void TileContextMenuClick(object sender, RoutedEventArgs e)
        {
            var mitem = sender as MenuItem;
            if (mitem == null) return;
            TileTypes res;
            if (!Enum.TryParse(mitem.Header as string, true, out res)) return;

            _selectedTileType = res;
        }

        private void RoomEditor_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var mpos = e.GetPosition(BaseCanvas);
            var index =
                (int)
                    (Math.Floor(mpos.Y/_tilesize)*Math.Ceiling(_room.Image.Source.Width/_tilesize) +
                     Math.Floor(mpos.X/_tilesize));
            if (index >= 0 && index < _room.Tiles.Length) _room.Tiles[index].TileType = _selectedTileType;
            _lMouseButtonDown = true;
        }

        private void InitGrid()
        {
            int x = 0, y = 0;

            foreach (var roomTile in _room.Tiles)
            {
                if (roomTile.Rectangle == null)
                {
                    roomTile.Rectangle = new Rectangle
                    {
                        Stroke = Brushes.Red
                    };
                }
                roomTile.Rectangle.Width = _tilesize - 1;
                roomTile.Rectangle.Height = _tilesize - 1;
                roomTile.Rectangle.StrokeThickness = 2;
                

                BaseCanvas.Children.Add(roomTile.Rectangle);
                Canvas.SetLeft(roomTile.Rectangle, x*_tilesize + 1);
                Canvas.SetTop(roomTile.Rectangle, y*_tilesize + 1);
                roomTile.Position = new Point(x, y);
                if (x++ >= (int) Math.Floor(_room.Image.Source.Width/_tilesize))
                {
                    y++;
                    x = 0;
                }
            }
        }

        private void MenuImportClick(object sender, RoutedEventArgs e)
        {
            var fdg = new OpenFileDialog {DefaultExt = "*.png"};
            fdg.ShowDialog();
            var filename = fdg.FileName;
            if (string.IsNullOrWhiteSpace(filename)) return;
            _room.Image.Source = new BitmapImage(new Uri(filename, UriKind.Absolute));
            _room.Image = _room.Image;
            _room.Tiles =
                new Tile[
                    (int)
                        (Math.Ceiling(_room.Image.Source.Width/_tilesize)*
                         Math.Ceiling(_room.Image.Source.Height/_tilesize))];
            for (var i = 0; i < _room.Tiles.Length; i++) _room.Tiles[i] = new Tile() {TileType = TileTypes.Floor};

            UpdateRoomDrawing();
        }

        private void MenuOpenClick(object sender, RoutedEventArgs e)
        {
            var fdg = new OpenFileDialog {Title = "Select room data file", DefaultExt = "*.mkr"};
            fdg.ShowDialog();

            if (string.IsNullOrWhiteSpace(fdg.FileName)) return;
            _room = FileHandler.Load(fdg.FileName);
            if (_room == null) return;
            fdg = new OpenFileDialog {Title = "Select room image file", DefaultExt = "*.png"};
            fdg.ShowDialog();
            if (string.IsNullOrWhiteSpace(fdg.FileName)) return;
            _room.Image.Source = new BitmapImage(new Uri(fdg.FileName));
            UpdateRoomDrawing();
        }

        private void MenuSaveClick(object sender, RoutedEventArgs e)
        {
            var fdg = new SaveFileDialog();
            fdg.ShowDialog();
            var filename = fdg.FileName;
            // _room.Tiles = _roomTiles;
            if (string.IsNullOrWhiteSpace(filename)) return;
                FileHandler.Save(_room, filename);
            UpdateRoomDrawing();
        }

        private void UpdateRoomDrawing()
        {
            BaseCanvas.Children.Clear();
            BaseCanvas.Children.Add(_room.Image);
            Width = _room.Image.Source.Width + 40;
            Height = _room.Image.Source.Height + 90;
            InitGrid();
        }
    }
}