using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogueController : MonoBehaviour {

	public const float OPENED_SCALE = 1f;
	public const float OPENED_BACKDROP_ALPHA = 0.3f;

	public const float CLOSED_SCALE = 0f;
	public const float CLOSED_BACKDROP_ALPHA = 0.0f;

	public Vector2 focusPoint;
	public GameObject dialoguePanel;
	public Text titleText;
	public Text mainText;

	public float ANIMATION_DURATION = 0.2f;
	public float animationSeconds = 0.0f;
	public Vector2 dialogueCenterPosition;

	public float targetScale;
	public float targetBackdropAlpha;
	public Vector2 targetPosition;

	public float startScale;
	public float startBackdropAlpha;
	public Vector2 startPosition;

	private bool isAnimating = false;
	// Use this for initialization
	void Start () {
		this.dialoguePanel.transform.localScale = new Vector3 (CLOSED_SCALE, CLOSED_SCALE, this.dialoguePanel.transform.localScale.z);

		this.dialogueCenterPosition = this.dialoguePanel.transform.position;
		//this.dialoguePanel.transform.position = focusPoint;
		Image backdrop = this.GetComponent<Image> ();
		backdrop.color = new Color (backdrop.color.r, backdrop.color.g, backdrop.color.b, CLOSED_BACKDROP_ALPHA);
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
			this.dialoguePanel.transform.localScale = new Vector2 (newScale, newScale);

			float newBackdropAlpha = Mathf.Lerp (this.startBackdropAlpha, this.targetBackdropAlpha, t);
			Image backdrop = this.GetComponent<Image> ();
			backdrop.color = new Color (backdrop.color.r, backdrop.color.g, backdrop.color.b, newBackdropAlpha);

			Vector2 newPosition = Vector2.Lerp (this.startPosition, this.targetPosition, t);
			this.dialoguePanel.transform.position = newPosition;
		}
	}

	public void initDialogue(string title, string text){
		this.titleText.text = title;
		this.mainText.text = text;
	}

	public void openDialogue(){
		this.animationSeconds = 0f;
		this.isAnimating = true;
		this.startBackdropAlpha = CLOSED_BACKDROP_ALPHA;
		this.targetBackdropAlpha = OPENED_BACKDROP_ALPHA;
		this.startScale = CLOSED_SCALE;
		this.targetScale = OPENED_SCALE;
		this.startPosition = this.focusPoint;
		this.targetPosition = this.dialogueCenterPosition;
	}

	public void closeDialogue(){
		this.animationSeconds = 0f;
		this.isAnimating = true;
		this.startBackdropAlpha = OPENED_BACKDROP_ALPHA;
		this.targetBackdropAlpha = CLOSED_BACKDROP_ALPHA;
		this.startScale = OPENED_SCALE;
		this.targetScale = CLOSED_SCALE;
		this.startPosition = this.dialogueCenterPosition;
		this.targetPosition = this.focusPoint;
	}
}
