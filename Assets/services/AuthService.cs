using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using RSG;
using UnityEngine.Networking;

public class AuthService : BaseService<AuthToken>{
	private AuthToken token = null;
	private string AUTH_TOKEN_PERSIST_PATH;

	void Start(){
		//AttributeTargets.
		this.AUTH_TOKEN_PERSIST_PATH = Application.persistentDataPath + "/auth.dat";
	}
	public IPromise<AuthToken> login(string username, string password) {
		JSONObject loginObject = new JSONObject ();
		loginObject.addProperty (new JSONProperty ("username", username));
		loginObject.addProperty (new JSONProperty ("password", password));

		//AuthToken persistedToken = this.loadPersistedAuthToken();
		//if (persistedToken != null) {
		//	return new Promise<AuthToken>((resolve, reject) => {
		//		resolve(persistedToken);
		//	});
		//}

		return new Promise<AuthToken>((resolve, reject) => {
			StartCoroutine(this.postRequestCoroutine(this.getServiceEndPoint() + "/login?include=user", loginObject.toJSONString(), resolve, reject));
		}).Then(authToken => {
			this.setAuthToken(authToken);
			this.persistAuthToken(authToken);
		});
	}

	public void setAuthToken (AuthToken token) {
		Debug.Log ("Auth token set");
		this.token = token;
	}
	public AuthToken getAuthToken () {
		return this.token;
	}

	public void signRequest(UnityWebRequest request){
		Debug.Log ("Signed request");
		if (this.token != null) {
			request.SetRequestHeader ("Authorization", this.token.id);
		}
	}

	public void persistAuthToken(AuthToken authToken) {
		Debug.Log ("Auth token persisted");
		BinaryFormatter binaryFormatter = new BinaryFormatter ();
		FileStream file = File.Open (AUTH_TOKEN_PERSIST_PATH, FileMode.Create);
		binaryFormatter.Serialize (file, authToken);
		file.Close ();
	}

	public AuthToken loadPersistedAuthToken() {
		if (!File.Exists (AUTH_TOKEN_PERSIST_PATH)) {
			return null;
		}

		BinaryFormatter binaryFormatter = new BinaryFormatter ();
		FileStream file = File.Open (AUTH_TOKEN_PERSIST_PATH, FileMode.Open);
		AuthToken authToken = (AuthToken) binaryFormatter.Deserialize (file);
		file.Close ();

		Debug.Log ("Auth token succesfully loaded");

		this.setAuthToken (authToken);

		return authToken;
	}
}


