using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class SideMenuController : MonoBehaviour, IEndDragHandler, IBeginDragHandler, IDragHandler {

	public GameObject overlay;
	public const float OVERLAY_MAX_VALUE = 0.7f;

	public const float ANIMATION_DURATION = 0.2f;
	public const int INVISIBLE_PIXELS_WIDTH = 20;
	public const int MIN_TRIGGER_SWIPE_LENGTH = 50;
	public float animationSeconds = 0.0f;
	public float targetX = 0.0f;
	public float startX = 0.0f;

	private float dragBeginX = 0.0f;
	private float dragBeginOverlayAlpha = 0.0f;
	private float startOverlayAlpha = 0.0f;
	public float targetOverlayAlpha = 0.0f;
	private bool isMoving = false;

	// Use this for initialization
	void Start () {
		this.transform.position = new Vector2 (this.getLeftLimit(), this.transform.position.y);
		this.dragBeginX = this.getLeftLimit();
		this.dragBeginOverlayAlpha = 0.0f;
		this.overlay.GetComponent<Image> ().color = new Color (0, 0, 0, 0);
		this.isMoving = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (targetX != 0.0f && this.isMoving) {
			animationSeconds += Time.deltaTime;
			float t = animationSeconds / ANIMATION_DURATION;
			if (t > 1.0f) {
				t = 1.0f;
				this.isMoving = false;
				this.setOverlayClickable ();
			} else {
				t = t * t * t * (t * (6f * t - 15f) + 10f);
			}
			float newX = Mathf.Lerp (this.startX, this.targetX, t);
			this.transform.position = new Vector2 (newX, this.transform.position.y);
			float newOverlayAlpha = Mathf.Lerp (this.startOverlayAlpha, this.targetOverlayAlpha, t);
			this.overlay.GetComponent<Image>().color = new Color(0,0,0, newOverlayAlpha);
		}
	}

	public void OnEndDrag (PointerEventData eventData) {
		float swipeLength = eventData.position.x - eventData.pressPosition.x;
		if (swipeLength > 50) {
			this.startMoving (this.getRightLimit (), OVERLAY_MAX_VALUE);
		} else if (swipeLength < -50) {
			this.startMoving (this.getLeftLimit (), 0.0f);
		} else if (Mathf.Abs (swipeLength) < 50) {
			this.startMoving (this.dragBeginX, this.dragBeginOverlayAlpha);
		}
	}

	public void startMoving(float targetX, float targetOverlayAlpha) {
		this.animationSeconds = 0.0f;
		this.targetX = targetX;
		this.targetOverlayAlpha = targetOverlayAlpha;
		this.startX = this.transform.position.x;
		this.startOverlayAlpha = this.overlay.GetComponent<Image> ().color.a;
		this.isMoving = true;
	}


	public void OnBeginDrag (PointerEventData eventData) {
		this.dragBeginX = this.transform.position.x;
		this.dragBeginOverlayAlpha = this.overlay.GetComponent<Image> ().color.a;
	}

	public void OnDrag(PointerEventData eventData){
		float swipeLength = eventData.position.x - eventData.pressPosition.x;
		float currentPositionX = this.dragBeginX + swipeLength;
		if ( currentPositionX >= this.getLeftLimit () && currentPositionX <= this.getRightLimit ()) {
			this.transform.position = new Vector2 (currentPositionX, this.transform.position.y);
			this.overlay.GetComponent<Image>().color = 
				new Color(0,0,0, Mathf.Lerp(0.0f, OVERLAY_MAX_VALUE, 
					(currentPositionX - this.getLeftLimit())/(this.getRightLimit() - this.getLeftLimit())));
		}
	}

	public float getLeftLimit(){
		float width = this.GetComponent<RectTransform> ().rect.width;
		return - width / 2 + 20;
	}

	public float getRightLimit(){
		float width = this.GetComponent<RectTransform> ().rect.width;
		return width / 2 - 1;
	}

	public void setOverlayClickable() {
		this.overlay.GetComponent<Image> ().raycastTarget = this.targetOverlayAlpha == OVERLAY_MAX_VALUE;
	}
}
