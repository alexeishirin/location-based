using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System;

public class Area : Location{

	public float width;
	public float height;

	public Area(LocationType locationType, string name, string description, float inTilePositionX, float inTilePositionY, List<ItemType> droppedItemTypes, float width, float height) : 
	base (locationType, name, description, inTilePositionX, inTilePositionY, droppedItemTypes) {
		this.width = width;
		this.height = height;
	}

	public Area(LocationType locationType, string name, string description, float inTilePositionX, float inTilePositionY, float width, float height) : 
	base (locationType, name, description, inTilePositionX, inTilePositionY) {
		this.width = width;
		this.height = height;
	}

	public override string ToString ()
	{
		return "Area:";
	}
}


