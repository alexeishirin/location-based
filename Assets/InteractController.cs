using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class InteractController : MonoBehaviour {

	public Text statusText;
	public GameObject statusPanel;

	public List<GameObject> objectsWithinReach = new List<GameObject> ();
	public List<GameObject> locationsWithinReach = new List<GameObject> ();
	public List<GameObject> areasInEffect = new List<GameObject> ();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}
}
