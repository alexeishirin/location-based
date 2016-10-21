using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class AvatarController : MonoBehaviour{
	float currentLerpTime = 1.0f;
	float lerpTime = 1.0f;

	Vector3 destination = new Vector3 (0.0f, 2.0f, -1);
	Vector3 lerpStartPosition = new Vector3 (0.0f, 2.0f, -1);

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		currentLerpTime += Time.deltaTime;
		if (currentLerpTime > lerpTime) {
			currentLerpTime = lerpTime;
		}
		if (currentLerpTime != lerpTime) {
			float t = currentLerpTime / lerpTime;
			t = t * t * t * (t * (6f * t - 15f) + 10f);
			this.transform.position = Vector3.Lerp (this.lerpStartPosition, this.destination, t);
		}
	}

	public void setNewDestination (Vector2 newDestination) {
		this.destination = new Vector3(newDestination.x, newDestination.y, this.transform.position.z);
		this.lerpStartPosition = this.transform.position;
		this.currentLerpTime = 0f;
		Debug.Log (this.destination);
	}
}
