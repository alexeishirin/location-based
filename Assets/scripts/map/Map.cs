using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System;

public class Map {

	public List<Hex> hexes = new List<Hex>();

	public Map(){


	}

	public void initMap() {
		this.loadHexes ();
		this.loadLocations ();
	}

	public void loadLocations() {
	}


	public void loadHexes(){
		List<ItemType> standardDropItemTypes = new List<ItemType> ();
		standardDropItemTypes.Add (ItemType.MODULE);
		for(int y = 1; y <= 20; y++){
			for (int x = 1; x <= 20; x++) {
				Hex newHex = new Hex (HexType.ROAD_HEX, x, y, "", standardDropItemTypes);
				this.hexes.Add(newHex);
				if (x == 9 && y == 9) {
					newHex.locations.Add (new Location (LocationType.DROP_LOCATION, "First", "First location blah-blah", 0.2f, 0.2f));
				}
				if (x == 8 && y == 9) {
					newHex.locations.Add (new Area (LocationType.DROP_LOCATION, "Area", "Ancient battle field", 0.5f, 0.5f, 0.8f, 0.8f));
				}
			}
		}
		
	}

	public Hex getHex(int x, int y) {
		foreach (Hex hex in this.hexes) {
			if (hex.x == x && hex.y == y) {
				return hex;
			}
		}

		return null;
	}

	public override string ToString ()
	{
		return "Map:" + JsonUtility.ToJson(this);
	}
}


