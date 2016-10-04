﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System;

public class Location {

	public LocationType locationType;
	//from 0.0 to 1.0
	public float inTilePositionX = 0.0f;
	public float inTilePositionY = 0.0f;
	public string name;
	public string description;
	public List<ItemType> droppedItemTypes = new List<ItemType>();

	public Location(LocationType locationType, string name, string description, float inTilePositionX, float inTilePositionY, List<ItemType> droppedItemTypes){
		this.locationType = locationType;
		this.name = name;
		this.description = description;
		this.inTilePositionX = inTilePositionX;
		this.inTilePositionY = inTilePositionY;
		this.droppedItemTypes = droppedItemTypes;
	}

	public Location(LocationType locationType, string name, string description, float inTilePositionX, float inTilePositionY){
		this.locationType = locationType;
		this.name = name;
		this.description = description;
		this.inTilePositionX = inTilePositionX;
		this.inTilePositionY = inTilePositionY;
		this.droppedItemTypes = Location.getDefaultDropItems (locationType);
	}

	public static List<ItemType> getDefaultDropItems(LocationType locationType){
		List<ItemType> itemTypes = new List<ItemType> ();
		itemTypes.Add (ItemType.MODULE);

		return itemTypes;
	}

	public Item dropItem() {
		float dropRate = 100.0f;

		float dropRoll = UnityEngine.Random.Range (0.0f, 100.0f);
		if (dropRoll <= dropRate) {
			ItemType itemType = this.droppedItemTypes[UnityEngine.Random.Range(0, this.droppedItemTypes.Count)];
			return new Item (itemType);
		}

		return null;
	}

	public override string ToString ()
	{
		return "Location:";
	}
}


