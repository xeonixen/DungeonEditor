﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace DungeonEditorV2
{
    public class FileHandler
    {
        /// <summary>
        /// Format
        /// Header:Position.X:Y:#Tiles:Tiles[]:imagepath
        /// </summary>
        const string Header = "NdNdv1";
        public static Room Load(string filename)
        {
            Room newRoom;
            if (!File.Exists(filename)) return null;
            using (var stream = File.Open(filename, FileMode.Open))
            using (var reader = new BinaryReader(stream, Encoding.UTF8))
            {

                var hd = Encoding.UTF8.GetBytes(Header);
                var buffer = new byte[hd.Length];
                reader.Read(buffer, 0, buffer.Length);
                var readHeader = Encoding.UTF8.GetString(buffer);
                if (readHeader != Header) return null;

                //writer.Write(hd, 0, hd.Length);
                var posX = reader.ReadDouble();
                //writer.Write(room.Position.X);
                var posY = reader.ReadDouble();
                // writer.Write(room.Position.Y);
                var tilesLength = reader.ReadInt32();
                //writer.Write(room.Tiles.Length);
                newRoom = new Room(new Point(posX, posY))
                {
                    Tiles = new Tile[tilesLength]
                };

                for (var i = 0; i < tilesLength; i++)
                {
                    var tt = (TileTypes)reader.ReadInt32();
                    newRoom.Tiles[i] = new Tile() { TileType = tt };
                }
            }
            return newRoom;

        }
        public static bool Save(Room room, string filename)
        {
            using (var stream = File.Open(filename, FileMode.Create))
            using (var writer = new BinaryWriter(stream, Encoding.UTF8))
            {
                var hd = Encoding.UTF8.GetBytes(Header);
                writer.Write(hd, 0, hd.Length);
                writer.Write(room.Position.X);
                writer.Write(room.Position.Y);
                writer.Write(room.Tiles.Length);
                foreach (var tile in room.Tiles)
                {
                    var tt = (int)tile.TileType;
                    writer.Write(tt);
                }

            }
            return true;

        }


    }


}
