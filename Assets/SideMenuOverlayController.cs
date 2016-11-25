using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class SideMenuOverlayController : MonoBehaviour, IPointerClickHandler {

	public GameObject sideMenuPanel;

	public void OnPointerClick( PointerEventData eventData ){
		SideMenuController sideMenuController = sideMenuPanel.GetComponent<SideMenuController> ();
		sideMenuController.startMoving (sideMenuController.getLeftLimit (), 0.0f);
	}
}
