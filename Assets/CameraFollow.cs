using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	public GameObject player;       //Public variable to store a reference to the player game object


	private Vector3 offset;         //Private variable to store the offset distance between the player and camera

	// Use this for initialization
	void Start () 
	{
		//Calculate and store the offset value by getting the distance between the player's position and camera's position.
		offset = transform.position - player.transform.position;
	}

	// LateUpdate is called after Update each frame
	void LateUpdate () 
	{
		// Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
		Vector3 screenAvatarPosition = new Vector3(Screen.width / 2, Screen.height - Screen.width / 2, 0);
		Vector3 worldAvatarPosition = this.GetComponent<Camera> ().ScreenToWorldPoint (screenAvatarPosition);
		Vector3 diff = player.transform.position - new Vector3 (worldAvatarPosition.x, worldAvatarPosition.y, player.transform.position.z);
		transform.position = transform.position + diff;
	}
}
