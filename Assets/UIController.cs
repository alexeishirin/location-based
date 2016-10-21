using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour {

	public GameObject statusPanel;
	public GameObject debugConsole;
	public GameObject mainPanel;
	public GameObject choosePanel;
	public GameObject debugPanel;
	public Text debugLogPrefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void showMessage(string message) {
		this.statusPanel.GetComponent<StatusPanelController> ().showMessage (message);
	}

	public void addDebugLog(string logString, string stackTrace, LogType type) {
		Text newDebugLog = Instantiate (debugLogPrefab);
		newDebugLog.text = logString + " " + stackTrace;
		if (type == LogType.Warning) {
			newDebugLog.color = new Color32 (255, 165, 0, 255);
		} else if (type == LogType.Error || type == LogType.Exception) {
			newDebugLog.color = new Color32 (255, 0, 0, 255);
		}
		newDebugLog.transform.SetParent (debugConsole.transform);
	}

	public static UIController getInstance() {
		GameObject uiControllerObject = GameObject.FindGameObjectWithTag ("UIController");

		return uiControllerObject.GetComponent<UIController> ();
	}

	public void toggleMainPanel(){
		this.mainPanel.SetActive(!this.mainPanel.activeSelf);
	}

	public void toggleChoosePanel(){
		this.choosePanel.SetActive(!this.choosePanel.activeSelf);
	}

	public void toggleDebugPanel(){
		this.debugPanel.SetActive(!this.debugPanel.activeSelf);
	}
}
