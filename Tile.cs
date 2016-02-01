using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DungeonEditorV2
{
    public enum TileTypes
    {
        Floor,
        Wall,
        Door,
        None
    }

    public class Tile
    {
        
        private TileTypes _tileType;

        public TileTypes TileType
        {
            get { return _tileType; }
            set
            {
                _tileType = value;
                switch (TileType)
                {
                    case TileTypes.Floor:
                        Rectangle.Stroke=Brushes.Red;
                        break;
                    case TileTypes.Wall:
                        Rectangle.Stroke = Brushes.Blue;
                        break;
                    case TileTypes.Door:
                        Rectangle.Stroke = Brushes.Green;
                        break;
                    case TileTypes.None:
                        Rectangle.Stroke = Brushes.Gray;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public Point Position { get; set; }=new Point();
        public Rectangle Rectangle { get; set; }=new Rectangle();
    }
}
