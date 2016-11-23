using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System;
using RSG;
using UnityEngine.Networking;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class I18nService : MonoBehaviour {
	private static I18nService instance = null;
	private static string[] languages = new string[]{
		"English"
	};
	private static string[] languageFileNames = new string[]{
		"en.json"
	};

	private string language = languages[0];

	private string LANGUAGES_FOLDER_PATH;
	private string dictionary = "";

	void Start(){
		//AttributeTargets.
		this.LANGUAGES_FOLDER_PATH = Path.Combine(Application.persistentDataPath, "i18n");
	}

	public static I18nService getInstance () {
		if (instance == null)
		{
			var gameObject = new GameObject("I18nService");
			GameObject.DontDestroyOnLoad(gameObject);
			instance = gameObject.AddComponent<I18nService>();
		}

		return instance;
	}

	public void setLanguage(string language) {
		this.language = language;
	}

	public string getCurrentLanguage() {
		return this.language;
	}

	public Promise<bool> loadLanguageFile() {
		return new Promise<bool> ((resolve, reject) => {
			StartCoroutine (this.loadLanguage (I18nService.languageFileNames [0], resolve, reject));
		});
	}

	public IEnumerator loadLanguage(string languageFileName, Action<bool> resolve, Action<Exception> reject) {
		string languageFilePath = LANGUAGES_FOLDER_PATH + languageFileName;
		if (!File.Exists (languageFilePath)) {
			UnityWebRequest request = UnityWebRequest.Get ("http://derelict-92382.onmodulus.net/assets/i18n/" + languageFileName);
			yield return this.getFile (request, resolve, reject, languageFileName);
		} else {
				this.dictionary = "";
				FileStream file = File.Open (languageFilePath, FileMode.Open);
				StreamReader reader = new StreamReader (file);
				while (!reader.EndOfStream) {
					this.dictionary += reader.ReadLine ();
					yield return null;
				}
				file.Close ();
				resolve (true);
		}
	}

	public IEnumerator getFile(UnityWebRequest request, Action<bool> resolve, Action<Exception> reject, string languageFileName) {
		yield return request.Send (); // Allow the async operation to complete.

		try {
			if (request.isError || !string.IsNullOrEmpty (request.error)) {
				reject (new ApplicationException (request.error));
			} else {
				Debug.Log (request.downloadHandler.text);
				this.dictionary = request.downloadHandler.text;
				if(!Directory.Exists(this.LANGUAGES_FOLDER_PATH)) {
					System.IO.Directory.CreateDirectory(this.LANGUAGES_FOLDER_PATH);
				}
				FileStream file = File.Open (Path.Combine(this.LANGUAGES_FOLDER_PATH, languageFileName), FileMode.Create);
				StreamWriter writer = new StreamWriter(file);
				writer.WriteLine(request.downloadHandler.text);
				file.Close ();
				resolve (true);
			}
		} catch (Exception ex) {
			reject (ex);
		}
	}

	public string getString(string key) {
		return this.dictionary;
	}

}


