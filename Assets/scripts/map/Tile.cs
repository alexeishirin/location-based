using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System;

public class Tile {

	public TileType tileType;
	public int tileX = 0;
	public int tileY = 0;
	public string tileImagePath = "";
	public List<ItemType> droppedItemTypes = new List<ItemType>();
	public List<Location> locations = new List<Location>();

	public Tile(TileType tileType, int tileX, int tileY, string tileImagePath, List<ItemType> droppedItemTypes){
		this.tileType = tileType;
		this.tileX = tileX;
		this.tileY = tileY;
		this.tileImagePath = tileImagePath;
		this.droppedItemTypes = droppedItemTypes;
	}

	public override string ToString ()
	{
		return "Tile:";
	}
}


