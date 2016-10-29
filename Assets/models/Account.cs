using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System;
using RSG;
using UnityEngine.Networking;

[Serializable]
public class Account : BaseModel, IBaseModel {
	public string getModelAPIEndPoint() {
		return "accounts";
	}

	public string username;
	public string password;
	public string email;
	public int startingLatitude;
	public int startingLongitude;
	public int startingMapX;
	public int startingMapY;


	public override string ToString ()
	{
		return "Account:" + JsonUtility.ToJson(this);
	}
}


