using UnityEngine;
using System.Collections;

public class DropController : MonoBehaviour {

	public GameObject itemPrefab;

	void OnTriggerEnter2D(Collider2D collidedWithObject) {
		Debug.Log ("entered");
		GameObject tileObject = collidedWithObject.gameObject;
		if (tileObject.tag == "Tile") {
			TileController tileController = tileObject.GetComponent<TileController> ();
			Tile tile = tileController.tile;
			Item droppedItem = this.dropItem (tile);
			if (droppedItem != null) {
				GameObject item = (GameObject)Instantiate (itemPrefab);
				Renderer tileRenderer = tileController.GetComponent<Renderer> ();
				item.transform.position = new Vector3 (
					Random.Range (tileRenderer.bounds.min.x, tileRenderer.bounds.max.x),
					Random.Range (tileRenderer.bounds.min.y, tileRenderer.bounds.max.y),
					tileController.transform.position.z);
				item.GetComponent<ItemController> ().item = droppedItem;
			}
		}
	}

	public Item dropItem(Tile tile) {
		float dropRate = 100.0f;

		float dropRoll = Random.Range (0.0f, 100.0f);
		if (dropRoll <= dropRate) {
			ItemType itemType = tile.droppedItemTypes[Random.Range(0, tile.droppedItemTypes.Count)];
			return new Item (itemType);
		}
			
		return null;
	}
}
