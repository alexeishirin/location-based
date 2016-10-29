using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System;
using RSG;
using UnityEngine.Networking;

public class AccountService : BaseService<Account> {
	private static AccountService instance = null;

	public static AccountService getInstance () {
		if (instance == null)
		{
			var gameObject = new GameObject("AccountService");
			GameObject.DontDestroyOnLoad(gameObject);
			instance = gameObject.AddComponent<AccountService>();
		}

		return instance;
	}
}


