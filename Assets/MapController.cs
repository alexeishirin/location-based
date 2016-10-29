using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class MapController : MonoBehaviour {

	private AccountService accountService;
	private AuthService authService;

	float startLatitude = 0.0f;
	float startLongitude = 0.0f;
	float currentLatitude = 0.0f;
	float currentLongitude = 0.0f;

	int startX = 9;
	int startY = 9;

	float scaleFactor = 100000.0f;

	//float speed = 0.005f;

	public Transform avatar;

	public List<GameObject> tileControllers = new List<GameObject> ();
	public GameObject tilePrefab;

	public Text xyText;
	public Text latLongText;
	public Text scaleFactorText;

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
		accountService = AccountService.getInstance();
		authService = AuthService.getInstance ();

		authService.login ("DarkestDay", "Pool1580")
			.Then (value => {
				authService.setAuthToken(value);
				Debug.Log(authService.getAuthToken());
			})
			.Catch (exception => Debug.LogException (exception));

		//UserService.login ("DarkestDay", "Pool1580")
		//	.Then (value => Debug.Log (value))
		//	.Catch (exception => Debug.LogException (exception));
		if (this.map == null) {
			this.map = new Map ();
			this.map.initMap ();
		}
		
		avatar.transform.position = new Vector3 (centerPosition.x, centerPosition.y, -1);

		StartGPS ();
		xyText.text = "go";
		this.scaleFactorText.text = "Scale Factor: " + this.scaleFactor;
	}

	public void setZoomLevel(int newZoomLevel) {
		if (this.map == null) {
			this.map = new Map ();
			this.map.initMap ();
		}
		int startIndex = -newZoomLevel / 2;
		int endIndex = newZoomLevel / 2;
		if (newZoomLevel % 2 == 0) {
			startIndex += 1;
		}
		for (int y = startIndex; y <= endIndex; y++) {
			for (int x = startIndex; x <= endIndex; x++) {
				this.tileControllers.Add (this.createTile(x, y, newZoomLevel));
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		//avatar.transform.position = Vector3.Lerp (avatar.transform.position, new Vector3 (30.0f, 30.0f, avatar.transform.position.z), speed * 0.1f * Time.deltaTime);
		//if (this.startLatitude != 0.0f && this.startLongitude != 0.0f && avatar.transform.position != this.destination) {
		//	avatar.transform.position = Vector3.Lerp (avatar.transform.position, this.destination, speed * Time.deltaTime);
		//}	
	}

	void StartGPS () {
		StartCoroutine(_StartGPS());
	}

	IEnumerator _StartGPS ()
	{
		Debug.Log ("coroutine started");
		while (true) {
			xyText.text = Input.location.status.ToString();
			if (Input.location.status == LocationServiceStatus.Failed || Input.location.status == LocationServiceStatus.Stopped) {
				Input.location.Start ();
				xyText.text = "starting";
			}
			// First, check if user has location service enabled
			if (!Input.location.isEnabledByUser) {
				//Debug.Log ("not enabled");
				//yield break;
			} else {
				// Wait until service initializes
				int maxWait = 20;
				while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) {
					yield return new WaitForSeconds (1);
					maxWait--;
				}

				// Service didn't initialize in 20 seconds
				if (maxWait < 1) {
					Debug.Log ("Timed out");
					xyText.text ="Timed out";

					yield break;
				}

				// Connection has failed
				if (Input.location.status == LocationServiceStatus.Failed) {
					xyText.text ="Unable to determine device location";
					Debug.Log ("Unable to determine device location");
					yield break;
				} else {
					xyText.text = "got coords";
					// Access granted and location value could be retrieved
					if (this.startLatitude == 0.0f) {
						this.startLatitude = Input.location.lastData.latitude;
					}
					if (this.startLongitude == 0.0f) {
						this.startLongitude = Input.location.lastData.longitude;
					}
					currentLatitude = Input.location.lastData.latitude;
					currentLongitude = Input.location.lastData.longitude;
					//this.lerpStartPosition = avatar.transform.position;
					//this.destination = new Vector3(centerPosition.x + this.getCurrentMoveVector ().x, centerPosition.y - this.getCurrentMoveVector().y, avatar.transform.position.z);
					//this.currentLerpTime = 0f;
					latLongText.text = "Lat Long: " + currentLatitude + ", " + currentLongitude;
					xyText.text = "Distance: " + this.getCurrentMoveVector ().magnitude * 100 + "m";
					//debugText.text = Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp + tick + Input.location.status.ToString();
					//Debug.Log ("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
				}
			}
			yield return new WaitForSeconds (1);
		}
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
		this.scaleFactorText.text = "Scale Factor: " + this.scaleFactor;
	}

	public void multiplyScaleFactor(float times) {
		this.scaleFactor *= times;
		this.scaleFactorText.text = "Scale Factor: " + this.scaleFactor;
	}

	public GameObject createTile(int x, int y, int zoomLevel) {
		GameObject tile = (GameObject)Instantiate (tilePrefab, Vector3.zero, Quaternion.identity, this.transform);
		TileController tileController = tile.GetComponent<TileController> ();
		tileController.map = this.map;
		tileController.zoomLevel = zoomLevel;
		tileController.loadTile (this.startX + x, this.startY + y);
		tile.transform.position = new Vector2 (centerPosition.x + tile.GetComponent<Renderer> ().bounds.size.x * x, centerPosition.y - tile.GetComponent<Renderer> ().bounds.size.y * y);

		return tile;
	}
}
