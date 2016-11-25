using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class HexMapController : MonoBehaviour {

	float startLatitude = 0.0f;
	float startLongitude = 0.0f;
	float currentLatitude = 0.0f;
	float currentLongitude = 0.0f;

	int startX = 50;
	int startY = 50;

	float scaleFactor = 100000.0f;

	//float speed = 0.005f;

	public Transform avatar;

	public List<GameObject> hexControllers = new List<GameObject> ();
	public GameObject hexPrefab;

	Vector2 centerPosition = new Vector2 (0.0f, 2.0f);
	Vector3 destination = new Vector3 (0.0f, 2.0f, -1);
	Vector3 lerpStartPosition = new Vector3 (0.0f, 2.0f, -1);

	public Map map;

	private bool isPointerOverUIObject() {
		PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
		eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		List<RaycastResult> results = new List<RaycastResult>();
		EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
		return results.Count > 0;
	}

	// Use this for initialization
	void Start () {
		if (this.map == null) {
			this.map = new Map ();
			this.map.initMap ();
		}

		avatar.transform.position = new Vector3 (centerPosition.x, centerPosition.y, -1);
	}

	public void setZoomLevel(int newZoomLevel) {
		if (this.map == null) {
			this.map = new Map ();
			this.map.initMap ();
		}
		int startIndex = -newZoomLevel / 2;
		int endIndex = newZoomLevel / 2;

		for (int x = startIndex; x <= endIndex; x++) {
			for (int y = startIndex; y <= endIndex; y++) {
				if ((x - y) <= newZoomLevel / 2 && (x - y) >= -newZoomLevel / 2) {
					this.hexControllers.Add (this.createHex (x, y, newZoomLevel));
				}
			}
		}
	}

	void Update () {
		//avatar.transform.position = Vector3.Lerp (avatar.transform.position, new Vector3 (30.0f, 30.0f, avatar.transform.position.z), speed * 0.1f * Time.deltaTime);
		//if (this.startLatitude != 0.0f && this.startLongitude != 0.0f && avatar.transform.position != this.destination) {
		//	avatar.transform.position = Vector3.Lerp (avatar.transform.position, this.destination, speed * Time.deltaTime);
		//}	
	}

	Vector3 polarToCortesian (float latitude, float longitude) {
		Vector3 origin = new Vector3 (0, 0, 1);
		Quaternion rotation = Quaternion.Euler (latitude, longitude, 0);

		Vector3 resultVector = rotation * origin;
		return resultVector * scaleFactor;
	}

	Vector2 getStartXY (){
		Vector3 startVector3 = polarToCortesian (this.startLatitude, this.startLongitude);

		return new Vector2 (startVector3.x, startVector3.y);
	}
	Vector2 getCurrentXY () {
		Vector3 currentVector3 = polarToCortesian (this.currentLatitude, this.currentLongitude);

		return new Vector2 (currentVector3.x, currentVector3.y);
	}

	Vector2 getCurrentMoveVector(){
		return this.getCurrentXY() - this.getStartXY();
	}

	public void increaseScaleFactor(float byPercent) {
		this.scaleFactor += byPercent;
	}

	public void multiplyScaleFactor(float times) {
		this.scaleFactor *= times;
	}

	public GameObject createHex(int x, int y, int zoomLevel) {
		GameObject hex = (GameObject)Instantiate (hexPrefab, Vector3.zero, Quaternion.identity, this.transform);
		HexController hexController = hex.GetComponent<HexController> ();
		hexController.map = this.map;
		hexController.zoomLevel = zoomLevel;
		hexController.loadHex (this.startX + x, this.startY + y);
		hex.transform.position = centerPosition + this.getHexShiftVector (x, y, hex);

		return hex;
	}

	public Vector2 getHexShiftVector(int x, int y, GameObject hex) {
		float hexSizeX = hex.GetComponent<Renderer> ().bounds.size.x;
		Vector2 xStepVector = new Vector2 (hexSizeX, 0);
		Vector2 yStepVector = new Vector2 ( hexSizeX * Mathf.Cos(2 * Mathf.PI / 3), hexSizeX * Mathf.Sin(2 * Mathf.PI / 3));

		return x * xStepVector + y * yStepVector;
	}
}
