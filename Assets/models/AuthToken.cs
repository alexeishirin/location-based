using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System;
using RSG;
using UnityEngine.Networking;

[Serializable]
public class AuthToken : BaseModel, IBaseModel{
	public string getModelAPIEndPoint() {
		return "accounts";
	}

	public int ttl;
	public string created;
	public string userId;
	public Account user;


	public override string ToString ()
	{
		return "AuthToken:" + JsonUtility.ToJson(this);
	}
}


