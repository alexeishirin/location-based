﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TabHeaderController : MonoBehaviour {
	public float SWIPE_DURATION = 0.2f;
	public const float activeScale = 1.5f;
	public const float inactiveScale = 1.0f;

	public Color activeColor = new Color(0.7f, 1.0f, 0.7f, 1.0f);
	public Color inactiveColor = new Color(0.2f, 0.2f, 0.2f, 1.0f);

	public bool active = false;
	private float animationSeconds = 2.0f;
	private Color startColor;
	private Color targetColor;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (this.animationSeconds < SWIPE_DURATION) {
			animationSeconds += Time.deltaTime;
			float t = animationSeconds / SWIPE_DURATION;
			t = t * t * t * (t * (6f * t - 15f) + 10f);

			float newColorR = Mathf.Lerp (this.startColor.r, this.targetColor.r, t);
			float newColorG = Mathf.Lerp (this.startColor.g, this.targetColor.g, t);
			float newColorB = Mathf.Lerp (this.startColor.b, this.targetColor.b, t);
			float newColorA = Mathf.Lerp (this.startColor.a, this.targetColor.a, t);

			this.GetComponentInChildren<Text>().color = new Color (newColorR, newColorG, newColorB, newColorA);
		}

	}

	public void setActive(bool active) {
		if (this.active != active) {
			this.animationSeconds = 0.0f;
			this.active = active;
			if (active) {
				this.startColor = inactiveColor;
				this.targetColor = activeColor;
			} else {
				this.startColor = activeColor;
				this.targetColor = inactiveColor;
			}
		}
	}
}
