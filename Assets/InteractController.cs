using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class InteractController : MonoBehaviour {

	public Text statusText;

	public List<GameObject> objectsWithinReach = new List<GameObject> ();
	public List<GameObject> locationsWithinReach = new List<GameObject> ();
	public List<GameObject> areasInEffect = new List<GameObject> ();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.touchCount > 0 || Input.GetMouseButton(0)) {
			if (Input.GetMouseButton(0) || Input.GetTouch (0).phase == TouchPhase.Began) {
				//this.touchStartingPosition = Input.GetTouch (0).position;
				Vector2 ray = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				RaycastHit2D hit = Physics2D.Raycast (ray, (Input.mousePosition));

				if (hit.collider && hit.collider.gameObject.tag == "Item") {
					GameObject itemObject = hit.collider.gameObject;
					if (this.objectsWithinReach.Contains (itemObject)) {
						this.statusText.text = "Picked up item!";
						Destroy (itemObject);
					} else {
						this.statusText.text = "Too far away!";
					}
				}
				if (hit.collider && hit.collider.gameObject.tag == "Location") {
					GameObject itemObject = hit.collider.gameObject;
					if (this.objectsWithinReach.Contains (itemObject)) {
						
						Item droppedItem = itemObject.GetComponent<LocationController> ().location.dropItem();
						this.statusText.text = "Drop in location:" + droppedItem;
						//Destroy (itemObject);
					} else {
						this.statusText.text = "Too far away!";
					}
				}
			}
		}
	
	}

	void OnTriggerEnter2D(Collider2D collidedWithObject) {
		if (collidedWithObject.gameObject.tag == "Item" && !this.objectsWithinReach.Contains(collidedWithObject.gameObject)) {
			this.objectsWithinReach.Add (collidedWithObject.gameObject);
		}
		if (collidedWithObject.gameObject.tag == "Location" && !this.objectsWithinReach.Contains(collidedWithObject.gameObject)) {
			this.locationsWithinReach.Add (collidedWithObject.gameObject);
		}
		if (collidedWithObject.gameObject.tag == "Area" && !this.objectsWithinReach.Contains(collidedWithObject.gameObject)) {
			this.areasInEffect.Add (collidedWithObject.gameObject);
			this.statusText.text = "You entered area!";
		}
	}

	void OnTriggerLeave2D(Collider2D collidedWithObject) {
		if (collidedWithObject.gameObject.tag == "Item" && this.objectsWithinReach.Contains (collidedWithObject.gameObject)) {
			this.objectsWithinReach.Remove (collidedWithObject.gameObject);
		}
		if (collidedWithObject.gameObject.tag == "Location" && !this.objectsWithinReach.Contains (collidedWithObject.gameObject)) {
			this.locationsWithinReach.Add (collidedWithObject.gameObject);
		}
		if (collidedWithObject.gameObject.tag == "Area" && !this.objectsWithinReach.Contains (collidedWithObject.gameObject)) {
			this.areasInEffect.Add (collidedWithObject.gameObject);
		}
	}
}
