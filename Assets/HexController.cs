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
		if (false && avatar != null) {
			Vector2 toAvatarVector = new Vector2 (avatar.transform.position.x - transform.position.x, avatar.transform.position.y - transform.position.y);
			float tileSize = GetComponent<Renderer> ().bounds.size.x;
			float moveVectorX = 0.0f;
			float moveVectorY = 0.0f;
			int tileXdifference = 0;
			int tileYdifference = 0;
			if (toAvatarVector.x >= (zoomLevel / 2) * tileSize) {
				moveVectorX = zoomLevel * tileSize;
				tileXdifference = Mathf.RoundToInt(zoomLevel);
			} else if (toAvatarVector.x <= - (zoomLevel / 2) * tileSize) {
				moveVectorX = - zoomLevel * tileSize;
				tileXdifference = - Mathf.RoundToInt(zoomLevel);
			}
			if (toAvatarVector.y >= (zoomLevel / 2) * tileSize) {
				moveVectorY = zoomLevel * tileSize;
				tileYdifference = - Mathf.RoundToInt(zoomLevel);
			} else if (toAvatarVector.y <= - (zoomLevel / 2) * tileSize) {
				moveVectorY = - zoomLevel * tileSize;
				tileYdifference = + Mathf.RoundToInt(zoomLevel);
			}

			if (moveVectorX != 0.0f || moveVectorY != 0.0f) {
				transform.position = new Vector3 (transform.position.x + moveVectorX, transform.position.y + moveVectorY, transform.position.z);
				this.loadHex (this.mapX + tileXdifference, this.mapY + tileYdifference);
			}
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
		Debug.Log ("Loading hex:  " + hexX + ":" + hexY);
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
}
