using UnityEngine;
using UnityEngine.UI;

public class PinchZoom : MonoBehaviour
{
	public float perspectiveZoomSpeed = 0.5f;        // The rate of change of the field of view in perspective mode.
	public float orthoZoomSpeed = 0.5f;        // The rate of change of the orthographic size in orthographic mode.

	public Text statusText;

	public GameObject mapGameObject;
	public GameObject avatar;

	private float minOrthographicSize = 0.1f;
	private float maxOrthographicSize = 100f;

	private int zoomLevel = 4;

	void Start() {
		float hexSize = this.getHexSize ();
		minOrthographicSize = 0.7f * hexSize * Screen.height / (2.0f * Screen.width);
		maxOrthographicSize = 3 * hexSize * Screen.height / (2.0f * Screen.width);
		this.zoomLevel = 5;
		mapGameObject.GetComponent<HexMapController> ().setZoomLevel(this.zoomLevel);
		this.GetComponent<Camera> ().orthographicSize = 1.2f * hexSize * Screen.height / (2.0f * Screen.width);
	}

	void Update()
	{
		Camera camera = this.GetComponent<Camera> ();
		// If there are two touches on the device...
		if (Input.touchCount == 2)
		{
			// Store both touches.
			Touch touchZero = Input.GetTouch(0);
			Touch touchOne = Input.GetTouch(1);

			// Find the position in the previous frame of each touch.
			Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
			Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

			// Find the magnitude of the vector (the distance) between the touches in each frame.
			float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
			float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

			// Find the difference in the distances between each frame.
			float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;


			// If the camera is orthographic...
			camera.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;

			// Make sure the orthographic size never drops below zero.
			camera.orthographicSize = Mathf.Max(camera.orthographicSize, minOrthographicSize);
			camera.orthographicSize = Mathf.Min(camera.orthographicSize, maxOrthographicSize);
			int newZoomLevel = Mathf.CeilToInt(camera.orthographicSize / this.getHexSize ()) + 2;
			if (newZoomLevel != this.zoomLevel) {
				this.zoomLevel = newZoomLevel;
			}
			statusText.text = "" + camera.orthographicSize;
		}
	}

	public float getHexSize() {
		HexMapController mapController = mapGameObject.GetComponent<HexMapController> ();

		return mapController.hexPrefab.GetComponent<Renderer> ().bounds.size.x;
	}
}