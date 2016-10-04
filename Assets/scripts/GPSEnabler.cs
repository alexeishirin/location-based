using UnityEngine;
using System.Collections;

public class GPSEnabler : MonoBehaviour
{

	float startLatitude = 0.0f;
	float startLongitude = 0.0f;
	float currentLatitude = 0.0f;
	float currentLongitude = 0.0f;

	void Start() {
		StartCoroutine(_StartGPS());
	}

	void Update() {
		
	}


	IEnumerator _StartGPS ()
	{
		while (true) {
			if (Input.location.status == LocationServiceStatus.Failed || Input.location.status == LocationServiceStatus.Stopped) {
				Input.location.Start ();
			}
			// First, check if user has location service enabled
			if (!Input.location.isEnabledByUser) {
				Debug.Log ("not enabled");
				yield break;
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
					//debugText.text ="Timed out";
					yield break;
				}

				// Connection has failed
				if (Input.location.status == LocationServiceStatus.Failed) {
					//debugText.text ="Unable to determine device location";
					Debug.Log ("Unable to determine device location");
					yield break;
				} else {
					// Access granted and location value could be retrieved
					if (this.startLatitude == 0.0f) {
						this.startLatitude = Input.location.lastData.latitude;
					}
					if (this.startLongitude == 0.0f) {
						this.startLongitude = Input.location.lastData.longitude;
					}
					currentLatitude = Input.location.lastData.latitude;
					currentLongitude = Input.location.lastData.longitude;
					//debugText.text = Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp + tick + Input.location.status.ToString();
					Debug.Log ("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
				}
			}
		}
	}


}