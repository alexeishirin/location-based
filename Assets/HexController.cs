using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;


public class HexController : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler {

	public TextMesh hexText;
	public GameObject locationPrefab;
	public GameObject areaPrefab;

	public int mapX = -1;
	public int mapY = -1;

	public Map map;
	public Hex hex;

	public float zoomLevel = 3;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		GameObject avatar = GameObject.FindGameObjectWithTag ("Avatar");
		if (avatar != null) {
			Vector2 toAvatarVector = new Vector2 (avatar.transform.position.x - transform.position.x, avatar.transform.position.y - transform.position.y);
			AvatarController avatarController = avatar.GetComponent<AvatarController> ();
			if (avatarController != null && avatarController.currentHex != null) {
				if (this.hexDistance (avatarController.currentHex, this.hex) >= Mathf.CeilToInt(zoomLevel / 2)) {
					//Debug.Log (this.hex.x + ":" + this.hex.y + ":" + this.hexDistance (avatarController.currentHex, this.hex));
					this.shiftHex (avatarController);
				}
			}
			/*
			float hexSize = GetComponent<Renderer> ().bounds.size.x;
			int hexXdifference = 0;
			int hexYdifference = 0;

			if (toAvatarVector.x >= (zoomLevel / 2) * hexSize) {
				hexXdifference = Mathf.RoundToInt(zoomLevel);
			} else if (toAvatarVector.x <= - (zoomLevel / 2) * hexSize) {
				hexXdifference = - Mathf.RoundToInt(zoomLevel);
			}

			int xActualDifference = Mathf.Abs(Mathf.RoundToInt (toAvatarVector.x / hexSize));

			float yAxisProjection = toAvatarVector.y / Mathf.Cos (Mathf.PI / 6);

			if (yAxisProjection >= (zoomLevel / 2) * hexSize) {
				hexYdifference = Mathf.RoundToInt(zoomLevel);
			} else if (yAxisProjection <= - (zoomLevel / 2) * hexSize) {
				hexYdifference = - Mathf.RoundToInt(zoomLevel);
			}

			int yActualDifference = Mathf.Abs(Mathf.RoundToInt (yAxisProjection / hexSize));

			if (xActualDifference + yActualDifference >= 3) {
				//Debug.Log (this.mapX + ":" + this.mapY);
			}
			*/

			/*if (hexXdifference != 0 || hexYdifference != 0) {
				Vector2 xShiftVector = hexXdifference * this.getHexXStepVector ();
				Vector2 yShiftVector = hexYdifference * this.getHexYStepVector ();
				transform.position += (Vector3)xShiftVector + (Vector3) yShiftVector;
				this.loadHex (this.mapX + hexXdifference, this.mapY + hexYdifference);
			}*/
		}

	}

	public void OnPointerDown( PointerEventData eventData ){
	}

	public void OnPointerUp( PointerEventData eventData ){
	}

	public void OnPointerClick( PointerEventData eventData ){
		GameObject avatar = GameObject.FindGameObjectWithTag ("Avatar");
		if (avatar != null) {
			avatar.GetComponent<AvatarController> ().setNewDestination (Camera.main.ScreenToWorldPoint (Input.mousePosition));
		}
	}

	public void loadHex(int hexX, int hexY) {
		this.hex = this.map.getHex (hexX, hexY);
		this.mapX = hexX;
		this.mapY = hexY;
		this.hexText.text = hexX + ":" + hexY;
		foreach(Location location in this.hex.locations){
			GameObject locationObject = null;
			if (location is Area) {
				locationObject = (GameObject)Instantiate (areaPrefab, Vector3.zero, Quaternion.identity, this.transform);
				locationObject.GetComponent<AreaController> ().area = (Area)location;
			} else if (location is Location) {
				locationObject = (GameObject)Instantiate (locationPrefab, Vector3.zero, Quaternion.identity, this.transform);
				locationObject.GetComponent<LocationController> ().location = location; 
			}

			locationObject.transform.position = this.calculateLocationPosition(location);
		}
	}

	public Vector3 calculateLocationPosition(Location location) {
		float hexTop = transform.position.y + GetComponent<Renderer> ().bounds.size.y / 2;
		float hexLeft = transform.position.x - GetComponent<Renderer> ().bounds.size.x / 2;

		float locationX = hexLeft + GetComponent<Renderer> ().bounds.size.x * location.inHexPositionX;
		float locationY = hexTop - GetComponent<Renderer> ().bounds.size.y * location.inHexPositionY;

		return new Vector3 (locationX, locationY, this.transform.position.z);
	}

	public Vector2 getHexXStepVector() {
		float hexSizeX = this.GetComponent<Renderer> ().bounds.size.x;
		return hexSizeX * this.getHexGridXUnitVector ();
	}
	public Vector2 getHexYStepVector() {
		float hexSizeX = this.GetComponent<Renderer> ().bounds.size.x;
		return hexSizeX * this.getHexGridYUnitVector();
	}

	public Vector2 getHexGridXUnitVector() {
		return new Vector2 (1, 0);
	}

	public Vector2 getHexGridYUnitVector() {
		return new Vector2 (Mathf.Cos(2 * Mathf.PI / 3), Mathf.Sin(2 * Mathf.PI / 3));
	}

	public void shiftHex(AvatarController avatarController) {
		Vector2 avatarShift = avatarController.lastShift ();
		int avatarShiftX = Mathf.RoundToInt(avatarShift.x);
		int avatarShiftY = Mathf.RoundToInt(avatarShift.y);
		int dx = Mathf.Abs (avatarController.previousHex.x - this.hex.x);
		int dy = Mathf.Abs (avatarController.previousHex.y - this.hex.y);
		int xShift = avatarShiftX * (Mathf.RoundToInt(zoomLevel) - Mathf.Abs(avatarShiftX * dy - avatarShiftY * dx));
		int yShift = avatarShiftY * (Mathf.RoundToInt(zoomLevel) - Mathf.Abs(avatarShiftY * dx - avatarShiftX * dy));
		
		if (hexDistance (this.map.getHex (this.hex.x + xShift, this.hex.y + yShift), avatarController.currentHex) >= Mathf.CeilToInt (zoomLevel / 2)) {
			Debug.Log ("canceling shift");
			return;
		}

		Vector2 xShiftVector = xShift * this.getHexXStepVector ();
		Vector2 yShiftVector = yShift * this.getHexYStepVector ();
		transform.position += (Vector3)xShiftVector + (Vector3) yShiftVector;
		this.loadHex (this.hex.x + xShift, this.hex.y + yShift);
	}

	public int hexDistance (Hex hexOne, Hex hexTwo) {
		return 
			(Mathf.Abs (hexOne.x - hexTwo.x) +
				Mathf.Abs (hexOne.x - hexTwo.x - (hexOne.y - hexTwo.y)) +
				Mathf.Abs (hexOne.y - hexTwo.y)) / 2;
	}


}
