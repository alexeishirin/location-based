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
	private static AuthService instance = null;
	private AuthToken token = null;
	private string AUTH_TOKEN_PERSIST_PATH;

	public static AuthService getInstance () {
		if (instance == null)
		{
			var gameObject = new GameObject("AuthService");
			GameObject.DontDestroyOnLoad(gameObject);
			instance = gameObject.AddComponent<AuthService>();
			instance.AUTH_TOKEN_PERSIST_PATH = Application.persistentDataPath + "/auth.dat";
			Debug.Log (instance.AUTH_TOKEN_PERSIST_PATH);
		}

		return instance;
	}

	public IPromise<AuthToken> login(string username, string password) {
		JSONObject loginObject = new JSONObject ();
		loginObject.addProperty (new JSONProperty ("username", username));
		loginObject.addProperty (new JSONProperty ("password", password));

		return new Promise<AuthToken>((resolve, reject) => {
			AuthToken persistedToken = this.loadPersistedAuthToken();
			if (persistedToken != null) {
				Action<AuthToken> newResolve = actionHelper.createSequence(this.setAuthToken, resolve);
				newResolve(persistedToken);
			} else {
				Action<AuthToken> newResolve = actionHelper.createSequence(this.setAuthToken, this.persistAuthToken, resolve);
				StartCoroutine(this.postRequestCoroutine(this.getServiceEndPoint() + "/login?include=user", loginObject.toJSONString(), newResolve, reject));
			}
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

		return authToken;
	}
}


