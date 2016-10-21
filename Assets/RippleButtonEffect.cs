using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class RippleButtonEffect : MonoBehaviour, IPointerClickHandler {

	public Image mask;

	public float RIPPLE_DURATION = 0.5f;
	public float RIPPLE_START_ALPHA = 0.4f;
	public float RIPPLE_FINISH_SCALE = 8.5f;
	public Color PRESSED_COLOR = new Color (0.7f, 1.0f, 0.7f);
	public bool isPressStick = true;
	private float animationSeconds = 0.0f;

	public void Start() {
		this.animationSeconds = RIPPLE_DURATION;
		this.resetMask ();
	}

	public void OnPointerClick( PointerEventData eventData ){
		mask.transform.position = eventData.pressPosition;
		this.animationSeconds = 0.0f;
		this.resetMask ();
		if (this.isPressStick) {
			this.GetComponent<Image> ().color = this.PRESSED_COLOR;
		}
	}

	public void Update() {
		if (this.animationSeconds < RIPPLE_DURATION) {
			animationSeconds += Time.deltaTime;

			float newAlpha = Mathf.Lerp (RIPPLE_START_ALPHA, 0.0f, animationSeconds / RIPPLE_DURATION);
			float newScale = Mathf.Lerp (0.0f, RIPPLE_FINISH_SCALE, animationSeconds / RIPPLE_DURATION);

			mask.transform.localScale = new Vector3 (newScale, newScale, mask.transform.localScale.z);
			mask.GetComponent<Image>().color = new Color (1, 1, 1, newAlpha);
		}
	}

	private void resetMask () {
		mask.transform.localScale = new Vector3 (0.0f, 0.0f, mask.transform.localScale.z);
		mask.GetComponent<Image>().color = new Color (1, 1, 1, RIPPLE_START_ALPHA);
	}
}
