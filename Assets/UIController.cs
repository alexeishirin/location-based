using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour {

	public AuthService authService;
	public I18nService i18nService;

	public GameObject statusPanel;
	public GameObject debugConsole;
	public GameObject mainPanel;
	public GameObject choosePanel;
	public GameObject debugPanel;
	public GameObject loginScreen;
	public Text debugLogPrefab;

	// Use this for initialization
	void Start () {
		AuthToken authToken = authService.loadPersistedAuthToken ();

		if (true || authToken != null) {
			loginScreen.SetActive (false);
			//load data for the map
			Debug.Log("Loading data for the map");
		} else {
			loginScreen.SetActive (true);
		}

		i18nService.loadLanguageFile ()
			.Then (value => {
				Debug.Log(value);
			})
			.Catch (exception => Debug.LogException (exception));
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

	public void showMap(){
		loginScreen.SetActive (false);
		this.showMessage ("Successfully logged in");
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
