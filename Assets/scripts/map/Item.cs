using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System;

public class Item {

	public ItemType type;

	public Item(ItemType type){
		this.type = type;
	}

	public override string ToString ()
	{
		return "Item: " + this.type;
	}
}


