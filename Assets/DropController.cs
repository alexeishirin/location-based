using UnityEngine;
using System.Collections;

public class DropController : MonoBehaviour {

	public GameObject itemPrefab;

	void OnTriggerEnter2D(Collider2D collidedWithObject) {
		GameObject hexObject = collidedWithObject.gameObject;
		if (hexObject.tag == "Hex") {
			HexController hexController = hexObject.GetComponent<HexController> ();
			Hex hex = hexController.hex;
			Item droppedItem = this.dropItem (hex);
			if (droppedItem != null) {
				GameObject item = (GameObject)Instantiate (itemPrefab);
				Renderer hexRenderer = hexController.GetComponent<Renderer> ();
				item.transform.position = new Vector3 (
					Random.Range (hexRenderer.bounds.min.x, hexRenderer.bounds.max.x),
					Random.Range (hexRenderer.bounds.min.y, hexRenderer.bounds.max.y),
					hexController.transform.position.z);
				item.GetComponent<ItemController> ().item = droppedItem;
			}
		}
	}

	public Item dropItem(Hex hex) {
		float dropRate = 100.0f;

		float dropRoll = Random.Range (0.0f, 100.0f);
		if (dropRoll <= dropRate) {
			ItemType itemType = hex.droppedItemTypes[Random.Range(0, hex.droppedItemTypes.Count)];
			return new Item (itemType);
		}
			
		return null;
	}
}
