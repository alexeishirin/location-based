using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ItemController : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler {

	public Item item;
	public bool isWithinReach = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnPointerDown( PointerEventData eventData ){
	}

	public void OnPointerUp( PointerEventData eventData ){
	}

	public void OnPointerClick( PointerEventData eventData ){
		if (this.isWithinReach) {
			UIController.getInstance().showMessage("Picked up item");
			Destroy (gameObject);
		} else {
			UIController.getInstance().showMessage("Item is too far away");
		}
	}

	void OnTriggerEnter2D(Collider2D collidedWithObject) {
		if (collidedWithObject.gameObject.tag == "InteractRadius") {
			this.isWithinReach = true;
		}
	}

	void OnTriggerExit2D(Collider2D collidedWithObject) {
		if (collidedWithObject.gameObject.tag == "InteractRadius") {
			this.isWithinReach = false;
		}
	}
}
