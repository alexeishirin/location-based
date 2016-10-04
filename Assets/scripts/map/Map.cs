using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System;

public class Map {

	public List<Tile> tiles = new List<Tile>();

	public Map(){


	}

	public void initMap() {
		this.loadTiles ();
		this.loadLocations ();
	}

	public void loadLocations() {
	}


	public void loadTiles(){
		List<ItemType> standardDropItemTypes = new List<ItemType> ();
		standardDropItemTypes.Add (ItemType.MODULE);
		for(int y = 1; y <= 20; y++){
			for (int x = 1; x <= 20; x++) {
				Tile newTile = new Tile (TileType.ROAD_TILE, x, y, "", standardDropItemTypes);
				this.tiles.Add(newTile);
				if (x == 9 && y == 9) {
					newTile.locations.Add (new Location (LocationType.DROP_LOCATION, "First", "First location blah-blah", 0.2f, 0.2f));
				}
				if (x == 8 && y == 9) {
					newTile.locations.Add (new Area (LocationType.DROP_LOCATION, "Area", "Ancient battle field", 0.5f, 0.5f, 0.8f, 0.8f));
				}
			}
		}
		
	}

	public Tile getTile(int x, int y) {
		foreach (Tile tile in this.tiles) {
			if (tile.tileX == x && tile.tileY == y) {
				return tile;
			}
		}

		return null;
	}

	public override string ToString ()
	{
		return "Tile:";
	}
}


