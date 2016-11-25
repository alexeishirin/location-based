using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class AreaController : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler {

	public Area area;
	public bool isAreaInEffect;

	// Use this for initialization
	void Start () {
		transform.localScale = this.calculateAreaScale ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public Vector3 calculateAreaScale () {
		float targetWidth = this.transform.parent.GetComponentInParent<Renderer> ().bounds.size.x * area.width;
		float targetHeight = this.transform.parent.GetComponentInParent<Renderer> ().bounds.size.y * area.height;

		float currentWidth = this.GetComponent<Renderer> ().bounds.size.x;
		float currentHeight = this.GetComponent<Renderer> ().bounds.size.y;

		float newLocalScaleX = this.transform.localScale.x / currentWidth * targetWidth;
		float newLocalScaleY = this.transform.localScale.y / currentHeight * targetHeight;

		return new Vector3 (newLocalScaleX, newLocalScaleY, transform.localScale.z);
	}

	public void OnPointerDown( PointerEventData eventData ){
	}

	public void OnPointerUp( PointerEventData eventData ){
	}

	public void OnPointerClick( PointerEventData eventData ){
	}

	void OnTriggerEnter2D(Collider2D collidedWithObject) {
		if (collidedWithObject.gameObject.tag == "InteractRadius") {
			this.isAreaInEffect = true;
			UIController.getInstance().showMessage("Entered the Area!");
		}
	}

	void OnTriggerExit2D(Collider2D collidedWithObject) {
		if (collidedWithObject.gameObject.tag == "InteractRadius") {
			this.isAreaInEffect = false;
			UIController.getInstance().showMessage("Left the Area!");
		}
	}
}
