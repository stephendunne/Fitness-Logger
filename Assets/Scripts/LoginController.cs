using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class LoginController : MonoBehaviour {

	public GameObject panelAccounts;
	public GameObject panelLogin;
	public GameObject panelRegister;


	public Text textLoginUsername;
	public Text textLoginPassword;

	public Text textRegisterUsername;
	public Text textRegisterPassword;
	public Text textRegisterRealname;
	public Text textRegisterHeight;
	public Text textRegisterWeight;
	public Text textRegisterTargetWeight;
	
	public UnitMeasurement selectedMeasurment;
	public Genders selectedGender;

	public Text labelMeasurementsTitle;
	public Text labelLoginTitle;
	public Text labelRegisterTitle;


	public void LoginUser()
	{
		bool foundUser = false;
		
		foreach (User u in AccountManager.accountManager.Users) 
		{
			if(u.Username == textLoginUsername.text && u.Password == textLoginPassword.text)
			{
				AccountManager.accountManager.ActiveUser = u;
				foundUser = true;
				panelAccounts.SetActive(false);
				GetComponent<MainController>().DEBUGChangeTab();
				labelLoginTitle.text = "LOGIN";
				textLoginUsername.text = "";
				textLoginPassword.text = "";
				GetComponent<MainController>().LoggedInUpdate();
				AccountManager.accountManager.SaveAccounts();
				break;
			}
		}
		
		if (!foundUser) 
		{
			labelLoginTitle.text = "No user found with this username or password. Try again";
		}
	}

	public void RegisterUser()
	{
		string u = textRegisterUsername.text;
		string pass = textRegisterPassword.text;
		string rn = textRegisterRealname.text;
		string h = textRegisterHeight.text;
		string w = textRegisterWeight.text;
		string tw = textRegisterTargetWeight.text;
		
		if (u == "" || pass == "" || rn == "" || h == "" || w == "" || tw == "") 
		{
			labelRegisterTitle.GetComponent<Text>().text = "Please fill in all fields below to register";
		} 
		else 
		{
			bool UserWithSameName = false;
			
			foreach (User user in AccountManager.accountManager.Users)
				if (user.Username == u)
					UserWithSameName = true;
			
			if (!UserWithSameName) 
			{
				float xHeight = float.Parse(h);
				float xWeight = float.Parse(w);
				float xTargetWeight = float.Parse(tw);


				if(selectedMeasurment == UnitMeasurement.Imperial)
				{
					xWeight = xWeight/ AccountManager.accountManager.ImperialWeightDivider; //Converting Pounds To Kilos
					xTargetWeight = xTargetWeight / AccountManager.accountManager.ImperialWeightDivider; //Converting Pounds To Kilos
					xHeight = xHeight / AccountManager.accountManager.ImperialHeightDivider; //Converting Inches to Centimetres
				}

				User xUser = new User (u, pass, rn, xHeight, xWeight, xTargetWeight, selectedGender, selectedMeasurment);
				AccountManager.accountManager.Users.Add (xUser);
				AccountManager.accountManager.ActiveUser = xUser;
				AccountManager.accountManager.SaveAccounts();
				panelAccounts.SetActive(false);
				labelRegisterTitle.text = "REGISTER";
				GetComponent<MainController>().LoggedInUpdate();
				AccountManager.accountManager.SaveAccounts();
			} 
			else
				labelRegisterTitle.GetComponent<Text> ().text = "Username taken. Try another";
		}
	}

	public void ChangePanel(GameObject panelToChangeTo)
	{
		labelLoginTitle.text = "LOGIN";
		labelRegisterTitle.text = "REGISTER";

		panelLogin.SetActive (false);
		panelRegister.SetActive (false);
		
		panelToChangeTo.SetActive (true);
	}

	//Dropdown List Related
	public Dropdown measurementsDropdown;
	public Dropdown gendersDropdown;

	void Start() {
		measurementsDropdown.onValueChanged.AddListener(delegate {
			mailboxDropdownValueChangedHandler(measurementsDropdown);
		});
		gendersDropdown.onValueChanged.AddListener(delegate {
			gendersDropdownValueChangedHandler(gendersDropdown);
		});

		if (AccountManager.accountManager.ActiveUser != null) {
			panelAccounts.SetActive (false);
			GetComponent<MainController> ().DEBUGChangeTab ();
		} 
		else 
		{
			panelAccounts.SetActive (true);	
		}
	}

	void Destroy() {
		measurementsDropdown.onValueChanged.RemoveAllListeners();
		gendersDropdown.onValueChanged.RemoveAllListeners();
	}
	
	private void mailboxDropdownValueChangedHandler(Dropdown target) {
		if (target.value == 0) 
		{
			labelMeasurementsTitle.text = "Enter in inches and pounds";
			selectedMeasurment = UnitMeasurement.Imperial;
		}
		
		if (target.value == 1) 
		{
			labelMeasurementsTitle.text = "Enter in centimetres and kilograms";
			selectedMeasurment = UnitMeasurement.Metric;
		}
	}

	private void gendersDropdownValueChangedHandler(Dropdown target) {
		if (target.value == 0) 
		{
			selectedGender = Genders.Male;
		}
		
		if (target.value == 1) 
		{
			selectedGender = Genders.Female;
		}
	}

	public void SetDropdownIndex(int index) {
		measurementsDropdown.value = index;
		gendersDropdown.value = index;
	}
	//END OF DROPDOWN RELATED METHODS
}
