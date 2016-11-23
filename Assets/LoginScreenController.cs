using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoginScreenController : MonoBehaviour {

	public AuthService authService;

	public Text loginInput;
	public Text passwordInput;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void login() {
		Debug.Log ("logging in");
		authService.login (loginInput.text, passwordInput.text)
			.Then (value => {
				Debug.Log(authService.getAuthToken());
				UIController.getInstance().showMap();
			})
			.Catch (exception => {
				Debug.LogException (exception);
				UIController.getInstance().showMessage(exception.Message);
			});
	}
}
