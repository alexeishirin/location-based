using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MapTrackController : MonoBehaviour {

	public Text statusText;
	public GameObject statusPanel;

	public List<GameObject> objectsWithinReach = new List<GameObject> ();
	public List<GameObject> locationsWithinReach = new List<GameObject> ();
	public List<GameObject> areasInEffect = new List<GameObject> ();

	public List<Hex> collidedWithHexes = new List<Hex> ();

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
	}

	void OnTriggerEnter2D(Collider2D collidedWithObject) {
		GameObject hexObject = collidedWithObject.gameObject;
		if (hexObject.tag == "Hex") {
			HexController hexController = hexObject.GetComponent<HexController> ();
			Hex hex = hexController.hex;
			GameObject avatar = GameObject.FindGameObjectWithTag ("Avatar");
			avatar.GetComponent<AvatarController> ().previousHex = avatar.GetComponent<AvatarController> ().currentHex;
			avatar.GetComponent<AvatarController> ().currentHex = hex;
			if(!this.collidedWithHexes.Contains(hex)) {
				this.collidedWithHexes.Add (hex);
			}
		}
	}

	void OnTriggerExit2D(Collider2D collidedWithObject) {
		GameObject hexObject = collidedWithObject.gameObject;
		if (hexObject.tag == "Hex") {
			HexController hexController = hexObject.GetComponent<HexController> ();
			Hex hex = hexController.hex;

			if (this.collidedWithHexes.Contains (hex)) {
				this.collidedWithHexes.RemoveAll (hexInList => hexInList == hex);
			}

			GameObject avatar = GameObject.FindGameObjectWithTag ("Avatar");
			if (avatar.GetComponent<AvatarController> ().currentHex == hex) {
				if (collidedWithHexes.Count > 0) {
					avatar.GetComponent<AvatarController> ().previousHex = avatar.GetComponent<AvatarController> ().currentHex;
					avatar.GetComponent<AvatarController> ().currentHex = collidedWithHexes[0];	
				}
			}
		}
	}
}
