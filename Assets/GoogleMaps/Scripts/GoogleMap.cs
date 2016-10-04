using UnityEngine;
using System.Collections;

public class GoogleMap : MonoBehaviour
{
	public enum MapType
	{
		RoadMap,
		Satellite,
		Terrain,
		Hybrid
	}
	public bool loadOnStart = true;
	public bool autoLocateCenter = true;
	public GoogleMapLocation centerLocation;
	public int zoom = 13;
	public MapType mapType;
	public int size = 512;
	public bool doubleResolution = false;
	public GoogleMapMarker[] markers;
	public GoogleMapPath[] paths;
	public GameObject spaceShip;
	public TextMesh debugText;
	private int tick = 1;
		
	void Start() {
		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer> ();
		spaceShip.transform.position = new Vector3(spriteRenderer.bounds.center.x, spriteRenderer.bounds.center.y, -1);
		if(loadOnStart) Refresh();	
	}

	//void Update() {
	//	Refresh ();
	//}

	public void Refresh() {
		if(autoLocateCenter && (markers.Length == 0 && paths.Length == 0)) {
			Debug.LogError("Auto Center will only work if paths or markers are used.");	
		}
		//if (!Input.location.isEnabledByUser) {
			// Start service before querying location
			Input.location.Start ();
		//}

		StartCoroutine(_Refresh());
	}
	
	IEnumerator _Refresh ()
	{
		while(true) {
			if (Input.location.status == LocationServiceStatus.Failed || Input.location.status == LocationServiceStatus.Stopped) {
				Input.location.Start ();
			}
			// First, check if user has location service enabled
		var latitude = 0.0f;
		var longitude = 0.0f;
		if (!Input.location.isEnabledByUser) {
			Debug.Log ("not enabled");
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
					debugText.text ="Timed out";
				yield break;
			}

			// Connection has failed
			if (Input.location.status == LocationServiceStatus.Failed) {
					debugText.text ="Unable to determine device location";
				Debug.Log ("Unable to determine device location");
				yield break;
			} else {
				// Access granted and location value could be retrieved
					latitude = Input.location.lastData.latitude;
					longitude = Input.location.lastData.longitude;
					debugText.text = Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp + tick + Input.location.status.ToString();
					tick++;
					//debugText.text = Input.location.status.ToString();
				Debug.Log ("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
			}

		}
		Debug.Log (zoom.ToString ());
		var url = "http://maps.googleapis.com/maps/api/staticmap";
		var qs = "";
		if (!autoLocateCenter) {
			if (centerLocation.address != "")
				qs += "center=" + WWW.UnEscapeURL (centerLocation.address);
			else {
					if(latitude == 0.0f) latitude = 53.90075f;
					if(longitude == 0.0f) longitude = 27.59056f;
					debugText.text = "lat=" + latitude + ",long=" + longitude;
				qs += "center=" + WWW.UnEscapeURL (string.Format ("{0},{1}", latitude, longitude));
			}
		
			qs += "&zoom=" + zoom.ToString ();
		}
		qs += "&size=" + WWW.UnEscapeURL (string.Format ("{0}x{0}", size));
		qs += "&scale=" + (doubleResolution ? "2" : "1");
		qs += "&maptype=" + mapType.ToString ().ToLower ();
		var usingSensor = false;
#if UNITY_IPHONE
		usingSensor = Input.location.isEnabledByUser && Input.location.status == LocationServiceStatus.Running;
#endif
		qs += "&sensor=" + (usingSensor ? "true" : "false");
		
		foreach (var i in markers) {
			qs += "&markers=" + string.Format ("size:{0}|color:{1}|label:{2}", i.size.ToString ().ToLower (), i.color, i.label);
			foreach (var loc in i.locations) {
				if (loc.address != "")
					qs += "|" + WWW.UnEscapeURL (loc.address);
				else
					qs += "|" + WWW.UnEscapeURL (string.Format ("{0},{1}", loc.latitude, loc.longitude));
			}
		}
		
		foreach (var i in paths) {
			qs += "&path=" + string.Format ("weight:{0}|color:{1}", i.weight, i.color);
			if(i.fill) qs += "|fillcolor:" + i.fillColor;
			foreach (var loc in i.locations) {
				if (loc.address != "")
					qs += "|" + WWW.UnEscapeURL (loc.address);
				else
					qs += "|" + WWW.UnEscapeURL (string.Format ("{0},{1}", loc.latitude, loc.longitude));
			}
		}

		//todo: adding style
		qs += "&style=hue:0xFF1A00|invert_lightness:true|saturation:-100|lightness:33|gamma:0.5";
		qs += "&style=feature:water|element:geometry|color:0x2D333C";

		Debug.Log (url + '?' + qs);
		var req = new WWW (url + '?' + qs);
		yield return req;
		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer> ();
			req.LoadImageIntoTexture (spriteRenderer.sprite.texture);
			req.Dispose ();
			req = null;

			yield return new WaitForSeconds (1);

		}

	}
	
	
}

public enum GoogleMapColor
{
	black,
	brown,
	green,
	purple,
	yellow,
	blue,
	gray,
	orange,
	red,
	white
}

[System.Serializable]
public class GoogleMapLocation
{
	public string address;
	public float latitude;
	public float longitude;
}

[System.Serializable]
public class GoogleMapMarker
{
	public enum GoogleMapMarkerSize
	{
		Tiny,
		Small,
		Mid
	}
	public GoogleMapMarkerSize size;
	public GoogleMapColor color;
	public string label;
	public GoogleMapLocation[] locations;
	
}

[System.Serializable]
public class GoogleMapPath
{
	public int weight = 5;
	public GoogleMapColor color;
	public bool fill = false;
	public GoogleMapColor fillColor;
	public GoogleMapLocation[] locations;	
}