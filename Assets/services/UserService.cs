using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System;
using RSG;
using UnityEngine.Networking;

public class UserService : MonoBehaviour{
	private static UserService instance = null;

	private static UserService getInstance() {
		if (instance == null) {
			GameObject gameObject = new GameObject("_UserService");
			GameObject.DontDestroyOnLoad(gameObject);
			instance = gameObject.AddComponent<UserService>();
		}

		return instance;
	}

	public static IPromise<string> login(string username, string password){
		string url = "http://derelict-92382.onmodulus.net/api/accounts/login?include=user";
		WWWForm data = new WWWForm ();
		data.AddField("username", username);
		data.AddField("password", password);
		return new Promise<string>((resolve, reject) =>
			UserService.getInstance().StartCoroutine(UserService.getInstance().TheCoroutine(url, data, resolve, reject))
		);
	}

	private IEnumerator TheCoroutine(string url, WWWForm data, Action<string> resolve, Action<Exception> reject)
	{
		
		UnityWebRequest www = UnityWebRequest.Post (url, data);
		Debug.Log ("send");

		yield return www.Send(); // Allow the async operation to complete.

		try
		{
			if (www.isError)
			{
				reject(new ApplicationException(www.error));
			}
			else
			{
				resolve(www.downloadHandler.text);
			}
		}
		catch (Exception ex)
		{
			reject(ex);
		}
	}
}


