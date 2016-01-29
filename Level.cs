using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace DungeonEditorV2
{
    public class Level
    {
        private readonly List<Room> _rooms = new List<Room>();
        private int _currentRoomIndex = -1;
        public List<string> AvailableRoomTemplates = new List<string>();
        public string Name { get; set; }
        public Size Size { get; set; }
        public int TileSize;

        public Level()
        {
            TileSize = 60;
            Size = new Size(2000,2000);
        }

        public Room CurrentRoom
        {
            get
            {
                if (_currentRoomIndex >= 0 && _currentRoomIndex < _rooms.Count) return _rooms[_currentRoomIndex];
                return null;
            }
            set { if (_rooms.Contains(value)) _currentRoomIndex = _rooms.IndexOf(value); }
        }

        public void SetCurrentRoom(Image image)
        {
            CurrentRoom.Selected = false;
            var room = _rooms.FirstOrDefault(a => a.Image == image);
            if (room != null) CurrentRoom = room;
            CurrentRoom.Selected = true;
        }

        public void AddRoom(Room room)
        {
            if (CurrentRoom != null) CurrentRoom.Selected = false;
            if (room != null) _rooms.Add(room);
            _currentRoomIndex = _rooms.Count - 1;
            if (CurrentRoom != null) CurrentRoom.Selected = true;
        }

        public string GetRoomData()
        {
            var sb = new StringBuilder();
            var i = 0;
            _rooms.ForEach(a => sb.Append($"{i++}: X:{a.Position.X} Y:{a.Position.Y}\n"));
            return sb.ToString();
        }
    }
}