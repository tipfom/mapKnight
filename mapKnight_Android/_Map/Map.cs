﻿using System;
using System.Collections.Generic;

namespace mapKnight_Android
{
	public class Map
	{
		// x, y, TileData (0 -> TileID, 1 -> Overlay, 2 -> MetaData)
		private ushort[,,] MapData;

		public int Width;
		public int Height;

		public string Author;
		public string Name;
		public int[] SpawnPoint;

		public Map (ushort[,,] Map, string author, string name, int[] spawnpoint)
		{
			if (Map.GetLength (2) == 3) {
				Width = Map.GetLength (0);
				Height = Map.GetLength (1);
				Author = author;
				Name = name;
				SpawnPoint = spawnpoint;

				MapData = Map;
			} else {
				throw new ArgumentException ("map has not the correct format");
			}
		}

		public Map (XMLElemental Map)
		{
			if (IsMapXML (Map)) {
				Author = Map.Attributes ["Author"];
				Name = Map.Attributes ["Name"];
				SpawnPoint = new int[2];
				int.TryParse (Map.Attributes ["Spawn"].Split (';') [0], out SpawnPoint [0]);
				int.TryParse (Map.Attributes ["Spawn"].Split (';') [1], out SpawnPoint [1]);

				int.TryParse (Map ["Data"].Attributes ["Width"], out Width);
				int.TryParse (Map ["Data"].Attributes ["Height"], out Height);

				MapData = new ushort[Width, Height, 3];

				// parse the def section
				Dictionary<string,ushort[]> DataValueIndex = new Dictionary<string, ushort[]> ();

				foreach (string Value in Map["Def"].Value.Split(new char[]{';'},StringSplitOptions.RemoveEmptyEntries)) {
					string[] Data = Value.Split (new char[]{ '=' }, StringSplitOptions.RemoveEmptyEntries) [1].Split (new char[]{ ',' }, StringSplitOptions.None);
					Tile lTile = (Tile)Enum.Parse (typeof(Tile), Data [0]);
					Overlay lOverlay;
					if (Data [1] != "")
						lOverlay = (Overlay)Enum.Parse (typeof(Overlay), Data [1]);
					else
						lOverlay = Overlay.None;

					DataValueIndex.Add (Value.Split (new char[]{ '=' }, StringSplitOptions.RemoveEmptyEntries) [0], new ushort[] {
						(ushort)lTile,
						(ushort)lOverlay
					});
				}

				// parse the data section
				int cX = 0;
				int cY = 0;

				foreach (string Data in Map["Data"].Value.Split(new char[]{','},StringSplitOptions.RemoveEmptyEntries)) {
					string[] AmountData = Data.Split (new char[]{ '~' }, StringSplitOptions.RemoveEmptyEntries);
					int Amount;
					if (int.TryParse (AmountData [0], out Amount)) {
						for (int i = 0; i < Amount; i++) {
							MapData [cX, cY, 0] = DataValueIndex [AmountData [1]] [0];
							MapData [cX, cY, 1] = DataValueIndex [AmountData [1]] [1];

							cX++;
							if (cX == Width) {
								cX = 0;
								cY++;
								if (cY == Height) {
									break;
								}
							}
						}
						if (cY == Height) {
							break;
						}
					}
				}
			}
		}

		public Tile GetTile (uint x, uint y)
		{
			return (Tile)MapData [x, y, 0];
		}

		public Overlay GetOverlay (uint x, uint y)
		{
			return (Width > x && Height > y) ? (Overlay)MapData [x, y, 1] : Overlay.None;
		}

		private static bool IsMapXML (XMLElemental MapXML)
		{
			if (MapXML ["Data"] != null &&
			    MapXML ["Data"].Attributes.ContainsKey ("Height") &&
			    MapXML ["Data"].Attributes.ContainsKey ("Width") &&
			    MapXML ["Def"] != null &&
			    MapXML ["Background"] != null &&
			    MapXML.Attributes.ContainsKey ("Author") &&
			    MapXML.Attributes.ContainsKey ("Name") &&
			    MapXML.Attributes.ContainsKey ("Spawn"))
				return true;
			
			return false;
		}
	}
}