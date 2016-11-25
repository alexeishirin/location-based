using UnityEngine;
using System.Collections;

public class Logger : MonoBehaviour {

	public string output = "";
	public string stack = "";
	void OnEnable() {
		Application.logMessageReceived += HandleLog;
	}
	void OnDisable() {
		Application.logMessageReceived -= HandleLog;
	}
	void HandleLog(string logString, string stackTrace, LogType type) {
		UIController.getInstance ().addDebugLog (logString, stackTrace, type);
		output = logString;
		stack = stackTrace;
	}
}
