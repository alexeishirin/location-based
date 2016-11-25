using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class StatusPanelController : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler {

	public Text statusText;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void showMessage(string message) {
		this.statusText.text = message;
		this.GetComponent<Animator> ().SetBool ("isVisible", true);
	}

	public void OnPointerDown( PointerEventData eventData ){
		this.GetComponent<Animator> ().SetBool ("isVisible", false);
	}

	public void OnPointerUp( PointerEventData eventData )
	{
	}

	public void OnPointerClick( PointerEventData eventData )
	{
		
	}
}
