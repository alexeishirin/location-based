using UnityEngine;
using System.Collections;


public class TileController : MonoBehaviour {

	public TextMesh tileText;
	public GameObject locationPrefab;
	public GameObject areaPrefab;

	public int mapX = -1;
	public int mapY = -1;

	public Map map;
	public Tile tile;

	public float zoomLevel = 3;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		GameObject avatar = GameObject.FindGameObjectWithTag ("Avatar");
		if (avatar != null) {
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
				this.loadTile (this.mapX + tileXdifference, this.mapY + tileYdifference);
			}
		}
	
	}

	public void loadTile(int tileX, int tileY) {
		this.tile = this.map.getTile (tileX, tileY);
		this.mapX = tileX;
		this.mapY = tileY;
		this.tileText.text = tileX + ":" + tileY;
		foreach(Location location in this.tile.locations){
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
		float tileTop = transform.position.y + GetComponent<Renderer> ().bounds.size.y / 2;
		float tileLeft = transform.position.x - GetComponent<Renderer> ().bounds.size.x / 2;

		float locationX = tileLeft + GetComponent<Renderer> ().bounds.size.x * location.inTilePositionX;
		float locationY = tileTop - GetComponent<Renderer> ().bounds.size.y * location.inTilePositionY;

		return new Vector3 (locationX, locationY, this.transform.position.z);
	
	}
}
