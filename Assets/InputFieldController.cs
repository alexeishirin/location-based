using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class InputFieldController : MonoBehaviour, ISelectHandler, IDeselectHandler {

	public GameObject underline;
	public GameObject label;

	public Color SELECTED_COLOR = new Color(0.3f, 0.3f, 1.0f);
	public Color INACTIVE_COLOR = new Color(0.7f, 0.7f, 0.7f);

	public const float SELECTED_LABEL_SCALE = 0.5f;
	public const float INACTIVE_LABEL_SCALE = 1.0f;

	private float startScale = 0.0f;
	public float targetScale = 0.0f; 

	private float startingLabelX = 0.0f;

	private Vector2 startPosition;
	public Vector2 targetPosition;

	public float ANIMATION_DURATION = 0.2f;
	private float animationSeconds = 0.0f;

	private bool isAnimating = false;


	// Use this for initialization
	void Start () {
		underline.GetComponent<Image> ().color = this.INACTIVE_COLOR; 
		label.GetComponent<Text> ().color = this.INACTIVE_COLOR;
		this.startPosition = label.transform.position;
		this.startScale = INACTIVE_LABEL_SCALE;

		Vector3[] corners = new Vector3[4];
		this.label.GetComponent<RectTransform> ().GetWorldCorners (corners);
		this.startingLabelX = corners [0].x;
		TouchScreenKeyboard.hideInput = true;
		this.GetComponent<InputField> ().shouldHideMobileInput = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (this.isAnimating) {
			animationSeconds += Time.deltaTime;
			float t = animationSeconds / ANIMATION_DURATION;
			if (t > 1.0f) {
				t = 1.0f;
				this.isAnimating = false;
			} else {
				t = t * t * t * (t * (6f * t - 15f) + 10f);
			}
			float newScale = Mathf.Lerp (this.startScale, this.targetScale, t);
			this.label.transform.localScale = new Vector2 (newScale, newScale);

			this.adjustLabelPosition (t);
		}

	}

	public void OnSelect(BaseEventData eventData){
		underline.GetComponent<Image> ().color = this.SELECTED_COLOR; 
		label.GetComponent<Text> ().color = this.SELECTED_COLOR;
		if (this.label.transform.localScale.x != SELECTED_LABEL_SCALE) {
			this.startAnimation (SELECTED_LABEL_SCALE);
		}
	}

	public void OnDeselect(BaseEventData eventData){
		underline.GetComponent<Image> ().color = this.INACTIVE_COLOR; 
		label.GetComponent<Text> ().color = this.INACTIVE_COLOR;
		if (this.label.transform.localScale.x != INACTIVE_LABEL_SCALE) {
			this.startAnimation (INACTIVE_LABEL_SCALE);
		}
	}

	public void startAnimation(float targetScale) {
		this.animationSeconds = 0.0f;
		this.startScale = this.label.transform.localScale.x;
		this.targetScale = targetScale;
		this.isAnimating = true;
	}

	public void adjustLabelPosition(float t) {
		Vector3[] labelCorners = new Vector3[4];
		this.label.GetComponent<RectTransform> ().GetWorldCorners (labelCorners);
		Vector3[] inputCorners = new Vector3[4];
		this.GetComponent<RectTransform> ().GetWorldCorners (inputCorners);
		float inputBottomY = inputCorners[0].y;
		float inputTopY = inputCorners [1].y;
		float inputHeight = Mathf.Abs (inputCorners [0].y - inputCorners [1].y);
		float labelTopY = labelCorners [1].y;
		float labelHeight = Mathf.Abs (labelCorners [0].y - labelCorners [1].y);

		float targetLabelY = this.targetScale == SELECTED_LABEL_SCALE ? inputTopY + labelHeight / 2 : inputTopY - inputHeight / 2;
		float startLabelY = this.targetScale == SELECTED_LABEL_SCALE ? inputTopY - inputHeight / 2 : inputTopY + labelHeight / 2;
		float labelXDifference = this.startingLabelX - labelCorners [0].x;
		Vector2 labelPosition = this.label.transform.position;
		labelPosition.x += labelXDifference;
		labelPosition.y = Mathf.Lerp (startLabelY, targetLabelY, t);
		this.label.transform.position = labelPosition;
	}
}
