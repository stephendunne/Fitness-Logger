using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class SettingsController : MonoBehaviour {

	public User user;

	public Text usernameLabel;

	public InputField pwInput;
	public InputField rnInput;
	public InputField hInput;
	public InputField twInput;
	public InputField caInput;
	public InputField wtInput;

	public Text passwordPlaceHolder;
	public Text passwordText;
	
	public Text realNamePlaceHolder;
	public Text realNameText;
	
	public Text heightPlaceHolder;
	public Text heightText;

	public Text targetWeightPlaceHolder;
	public Text targetWeightText;
	
	public Text calorieAllowancePlaceHolder;
	public Text calorieAllowanceText;
	
	public Text waterTargetPlaceHolder;
	public Text waterTargetText;

	public void SaveChanges()
	{
		user = AccountManager.accountManager.ActiveUser;

		if(passwordText.text != "")
			user.Password = passwordText.text;

		if(realNameText.text != "")
			user.Realname = realNameText.text;
		
		if(heightText.text != "")
			user.Height = float.Parse(heightText.text);
		
		if(targetWeightText.text != "")
			user.TargetWeight = float.Parse(targetWeightText.text);
		
		if(calorieAllowanceText.text != "")
			user.CalorieAllowance = Convert.ToInt32(calorieAllowanceText.text);
		
		if(waterTargetText.text != "")
			user.WaterTarget = Convert.ToInt32(waterTargetText.text);



		for (int i = 0; i < AccountManager.accountManager.Users.Count; i++) {
			if (AccountManager.accountManager.Users [i].Username == user.Username)
			{
				AccountManager.accountManager.Users [i] = user;
				AccountManager.accountManager.ActiveUser = user;
				GetComponent<MainController>().user = user;
				GetComponent<MainController>().UpdateDetailPanels();
				UpdatePlaceHolders (user);
				return;
			}
		}
	}

	public void UpdatePlaceHolders(User u)
	{
		usernameLabel.text = "Username: " + u.Username;

		pwInput.text = "";
		rnInput.text = "";
		hInput.text = "";
		twInput.text = "";
		caInput.text = "";
		wtInput.text = "";
		
		realNamePlaceHolder.text = u.Realname;
		heightPlaceHolder.text = String.Format("{0:f2}",u.Height);
		targetWeightPlaceHolder.text = String.Format("{0:f2}",u.TargetWeight);
		calorieAllowancePlaceHolder.text = String.Format("{0}",u.CalorieAllowance);
		waterTargetPlaceHolder.text = String.Format("{0}",u.WaterTarget);
	}

	public void Logout()
	{
		AccountManager.accountManager.ActiveUser = null;
		GetComponent<LoginController> ().panelAccounts.SetActive (true);
		AccountManager.accountManager.SaveAccounts();
	}
}
