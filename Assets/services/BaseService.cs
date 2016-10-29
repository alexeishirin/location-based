using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Text;
using RSG;
using UnityEngine.Networking;

public class BaseService<T> : MonoBehaviour where T: BaseModel, IBaseModel, new(){
	public static string API_END_POINT = "http://derelict-92382.onmodulus.net/api/";

	public IPromise<T> create(T newObject){
		string url = this.getServiceEndPoint();
		string objectAsJson = JsonUtility.ToJson (newObject);

		return new Promise<T>((resolve, reject) =>
			StartCoroutine(this.postRequestCoroutine(url, objectAsJson, resolve, reject))
		);
	}

	public IPromise<T> getById(string id){
		string url = this.getServiceEndPoint() + "/" + id;

		return new Promise<T>((resolve, reject) =>
			StartCoroutine(this.getRequestCoroutine(url, resolve, reject))
		);
	}

	public IPromise<T> update(T newObject){
		string url = this.getServiceEndPoint() + "/" + newObject.id;
		string objectAsJson = JsonUtility.ToJson (newObject);

		return new Promise<T>((resolve, reject) =>
			StartCoroutine(this.postRequestCoroutine(url, objectAsJson, resolve, reject))
		);
	}

	public IPromise<T> deleteById(string id){
		string url = this.getServiceEndPoint() + "/" + id;

		return new Promise<T>((resolve, reject) =>
			StartCoroutine(this.deleteRequestCoroutine(url, resolve, reject))
		);
	}

	public string getServiceEndPoint() {
		IBaseModel model = new T ();

		return API_END_POINT + model.getModelAPIEndPoint ();
	}


	public IEnumerator postRequestCoroutine(string url, string jsonData, Action<T> resolve, Action<Exception> reject)
	{
		WWWForm dummyForm = new WWWForm ();
		dummyForm.AddField ("stupid", "stuff");
		UnityWebRequest request = UnityWebRequest.Post (url, dummyForm);

		Debug.Log (jsonData);
		//Removing the id field if it is not set
		if (jsonData.IndexOf ("\"id\":\"\",") != -1) {
			Debug.Log ("removed id");
			jsonData = jsonData.Replace ("\"id\":\"\",", "");
		}
		Debug.Log (jsonData);

		byte[] data = System.Text.Encoding.UTF8.GetBytes(jsonData);

		UploadHandlerRaw upHandler = new UploadHandlerRaw(data);
		upHandler.contentType = "application/json";
		request.uploadHandler = upHandler;

		yield return this.sendRequest (request, resolve, reject);
	}

	public IEnumerator getRequestCoroutine(string url, Action<T> resolve, Action<Exception> reject)
	{
		UnityWebRequest request = UnityWebRequest.Get (url);
		yield return this.sendRequest (request, resolve, reject);
	}

	public IEnumerator deleteRequestCoroutine(string url, Action<T> resolve, Action<Exception> reject)
	{
		UnityWebRequest request = UnityWebRequest.Delete (url);
		yield return this.sendRequest (request, resolve, reject);
	}

	public IEnumerator sendRequest(UnityWebRequest request, Action<T> resolve, Action<Exception> reject) {
		request.SetRequestHeader("Content-Type", "application/json");
		AuthService.getInstance ().signRequest (request);
		yield return request.Send(); // Allow the async operation to complete.

		try
		{
			if (request.isError || !string.IsNullOrEmpty(request.error))
			{
				reject(new ApplicationException(request.error));
			}
			else
			{
				Debug.Log(request.downloadHandler.text);
				resolve(JsonUtility.FromJson<T>(request.downloadHandler.text));
			}
		}
		catch (Exception ex)
		{
			reject(ex);
		}
		
	}

	public class JSONProperty {
		public string key;
		public object value;

		public JSONProperty (string key, object value) {
			this.key = key;
			this.value = value;
		}

		public string toJSONString () {
			return "\"" + this.key + "\":\"" + this.value.ToString() + "\"";
		}
	}

	public class JSONObject {
		public List<JSONProperty> properties = new List<JSONProperty>();

		public void addProperty(JSONProperty property) {
			this.properties.Add (property);
		}

		public string toJSONString () {
			string jsonString = "{";

			foreach(JSONProperty property in this.properties) {
				jsonString += property.toJSONString();
				if (this.properties.IndexOf (property) != this.properties.Count - 1) {
					jsonString += ",";
				}
			}

			jsonString += "}";

			return jsonString;
		}
	}
}


