using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;

public class AccountManager : MonoBehaviour
{
	public static AccountManager accountManager;

	public List<User> Users = new List<User>();

	public User ActiveUser;

	public float ImperialWeightDivider = 0.393701f;
	public float ImperialHeightDivider = 2.20462f;

	public float MetricWeightMultiplier = 2.54f;
	public float MetricHeightMultiplier = 0.45f;

	void Awake()
	{
		if (accountManager == null) 
		{
			DontDestroyOnLoad(gameObject);
			accountManager = this;
		}

		else if (accountManager != this)
			Destroy (gameObject);

		LoadAccounts ();
	}

	public void SaveAccounts()
	{
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/accountsInfo2.dat");

		Accounts data = new Accounts ();
		data.Users = Users;
		data.ActiveUser = ActiveUser;

		bf.Serialize (file, data);
		file.Close ();
	}

	public void LoadAccounts()
	{
		if (File.Exists (Application.persistentDataPath + "/accountsInfo2.dat")) 
		{
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/accountsInfo2.dat", FileMode.Open);
			Accounts data = (Accounts)bf.Deserialize(file);
			file.Close();

			Users = data.Users;
			ActiveUser = data.ActiveUser;
		}
	}
}

[Serializable]
class Accounts
{
	public List<User> Users = new List<User>();
	public User ActiveUser;
}
