using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System;
using RSG;
using UnityEngine.Networking;

public class AuthService : BaseService<AuthToken>{
	private static AuthService instance = null;
	private AuthToken token = null;

	public static AuthService getInstance () {
		if (instance == null)
		{
			var gameObject = new GameObject("AuthService");
			GameObject.DontDestroyOnLoad(gameObject);
			instance = gameObject.AddComponent<AuthService>();
		}

		return instance;
	}

	public IPromise<AuthToken> login(string username, string password) {
		JSONObject loginObject = new JSONObject ();
		loginObject.addProperty (new JSONProperty ("username", username));
		loginObject.addProperty (new JSONProperty ("password", password));

		return new Promise<AuthToken>((resolve, reject) =>
			StartCoroutine(this.postRequestCoroutine(this.getServiceEndPoint() + "/login?include=user", loginObject.toJSONString(), resolve, reject))
		);
	}

	public void setAuthToken (AuthToken token) {
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
}


