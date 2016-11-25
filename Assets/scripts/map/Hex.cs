using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System;

public class Hex {

	public HexType hexType;
	public int x = 0;
	public int y = 0;
	public string hexImagePath = "";
	public List<ItemType> droppedItemTypes = new List<ItemType>();
	public List<Location> locations = new List<Location>();

	public Hex(HexType hexType, int x, int y, string hexImagePath, List<ItemType> droppedItemTypes){
		this.hexType = hexType;
		this.x = x;
		this.y = y;
		this.hexImagePath = hexImagePath;
		this.droppedItemTypes = droppedItemTypes;
	}

	public override string ToString ()
	{
		return "Hex:" + JsonUtility.ToJson(this);
	}
}


