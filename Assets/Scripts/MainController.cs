using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class MainController : MonoBehaviour {

	public Text titleText;

	public GameObject panelLogin;

	public GameObject buttonSettings;
	public GameObject panelSettings;

	public GameObject buttonDateSelect;

	public GameObject tabButtonBmi;
	public GameObject tabButtonCardio;
	public GameObject tabButtonDiet;
	public GameObject tabButtonWater;

	public GameObject panelBmi;
	public GameObject panelDiet;
	public GameObject panelWater;

	public GameObject panelLogBmi;
	public Text textBMILog;

	public GameObject panelLogCardio;
	public Text textCardioNameLog;
	public Text textCardioCaloriesLog;

	public GameObject panelLogDiet;
	public Text textDietNameLog;
	public Text textDietCaloriesLog;

	public Text BMIDescription;
	public Text WaterDescription;

	public User user;

	public DateTime CurrentDate = DateTime.Today;

	ScrollViewController scrollViewController;

	public Text graphWeightFirst;
	public Text graphWeightLast;
	public Text graphDateFirst;
	public Text graphDateLast;

	public Text textDietCalorieLabel;

	void Start () 
	{
		AccountManager.accountManager.Users.Add(new User("a","a","A A", 181,89,80,Genders.Male,UnitMeasurement.Metric));
		scrollViewController = GetComponent<ScrollViewController> ();
		ChangeTab (panelBmi);
	}

	public void LoggedInUpdate()
	{
			user = AccountManager.accountManager.ActiveUser;
			CurrentDate = DateTime.Today;
			UpdateDetailPanels();
	}

	public void PrevDay()
	{
		CurrentDate = CurrentDate.AddDays (-1);
		UpdateDetailPanels ();
	}

	public void NextDay()
	{
		CurrentDate = CurrentDate.AddDays (1);
		UpdateDetailPanels ();
	}

	public void UpdateDetailPanels()
	{
		for (int i = 0; i < AccountManager.accountManager.Users.Count; i++) {
			if (AccountManager.accountManager.Users [i].Username == user.Username)
			{
				AccountManager.accountManager.Users [i] = user;
			}
		}
		AccountManager.accountManager.SaveAccounts ();

		buttonDateSelect.GetComponentInChildren<Text> ().text = String.Format("{0:dd/MMM/yy}",CurrentDate);

		bool foundLog = false;

		foreach (DailyLog dl in user.Logs) {
			if (dl.Date == CurrentDate) 
			{
				UpdateBMIPanel (dl);

				int mealCaloriesTotal, exerciseCaloriesTotal;
				exerciseCaloriesTotal = scrollViewController.UpdateExerciseScrollViewPanel (dl);
				mealCaloriesTotal = scrollViewController.UpdateMealsScrollViewPanel (dl);
				UpdateCalorieAllowanceCount(mealCaloriesTotal, exerciseCaloriesTotal);

				UpdateWaterConsumedCount(dl);

				foundLog = true;
			}
		}

		if (!foundLog) 
		{
			BMIDescription.text = "";
			scrollViewController.ClearScrollViewPanels();
			textDietCalorieLabel.text = String.Format ("Allowed Calories Remaining: {0} - 0 + 0 = {0}\n(Daily Allowance - Consumed - Burned = Balance)", user.CalorieAllowance);
			WaterDescription.text = "Consumed: " + 0 + "/" + user.WaterTarget + "ml";
		}
	}

	void UpdateBMIPanel(DailyLog log)
	{
		float weightLost = user.StartWeight - log.Weight;
		float weightLeft = log.Weight - user.TargetWeight;

		float bmi = (log.Weight / (user.Height * user.Height)) * 10000;
		string bmiStatus;

		if (bmi >= 0 && bmi < 18)
			bmiStatus = "You are underweight";
		else if (bmi < 18.5)
			bmiStatus = "You are thin for your height";
		else if (bmi < 25 && bmi >= 18.5)
			bmiStatus = "You are a healthy weight";
		else if (bmi > 30)
			bmiStatus = "You are obese";
		else if (bmi >= 25)
			bmiStatus = "You are overweight for your height";

		else
			bmiStatus = "~Something went wrong here~";

		if(user.MeasurementPreference == UnitMeasurement.Metric)
			BMIDescription.text = String.Format("Weight: {0:f2}kgs\n\nWeight Lost: {1:f2}kgs\n{2:f2}kgs off your target\nBMI: {3:f2}\n{4}",
		                                	log.Weight, weightLost, +(weightLeft), bmi, bmiStatus);
		else
			BMIDescription.text = String.Format("Weight: {0:f2}lbs\n\nWeight Lost: {1:f2}lbs\n{2:f2}lbs off your target\nBMI: {3:f2}\n{4}",
			                                    log.Weight / AccountManager.accountManager.MetricWeightMultiplier, +(weightLost / AccountManager.accountManager.MetricWeightMultiplier), weightLeft / AccountManager.accountManager.MetricWeightMultiplier, bmi, bmiStatus);

		UpdateBMIGraph ();
	}

	void UpdateBMIGraph()
	{
		user.Logs.Sort((a,b) => a.Date.CompareTo(b.Date));

		graphWeightFirst.text = String.Format("{0}lbs",user.Logs [0].Weight);
		graphWeightLast.text = String.Format("{0}lbs",user.Logs[user.Logs.Count-1].Weight);
		graphDateFirst.text = String.Format("{0:dd/MMM/yy}",user.Logs[0].Date);
		graphDateLast.text = String.Format("{0:dd/MMM/yy}",user.Logs[user.Logs.Count-1].Date);
	}

	void UpdateCalorieAllowanceCount(int caloriesConsumed, int caloriesBurned)
	{
		int caloriesRemaining = user.CalorieAllowance - caloriesConsumed + caloriesBurned;
		textDietCalorieLabel.text = String.Format ("Allowed Calories Remaining: {0} - {1} + {2} = {3}\n(Daily Allowance - Consumed - Burned = Balance)", user.CalorieAllowance, caloriesConsumed, caloriesBurned, caloriesRemaining);
	}

	void UpdateWaterConsumedCount(DailyLog log)
	{
		WaterDescription.text = "Consumed: " + log.WaterConsumed + "/" + user.WaterTarget + "ml";
	}

	public void AddBMILog()
	{
		float xWeight;

		if (textBMILog.text != "") 
		{
			xWeight = float.Parse(textBMILog.text);
			if(user.MeasurementPreference == UnitMeasurement.Imperial)
				xWeight = xWeight / AccountManager.accountManager.ImperialWeightDivider;

			bool logFound = false;

			foreach (DailyLog dl in user.Logs) {
				if (dl.Date == CurrentDate) 
				{
					dl.Weight = xWeight;
					logFound = true;
				}
			}
			
			if (!logFound) {
				DailyLog log = new DailyLog();
				log.Weight = xWeight;
				log.Date = CurrentDate;
				user.Logs.Add(log);
			}

			panelLogBmi.SetActive(false);
			UpdateDetailPanels();
		}
	}

	public void AddCardioLog()
	{
		string xName;
		int xCalories;
		
		if (textCardioNameLog.text != "" && textCardioCaloriesLog.text != "") {
			xName = textCardioNameLog.text;
			xCalories = Convert.ToInt32(textCardioCaloriesLog.text);
		
			bool logFound = false;
		
			foreach (DailyLog dl in user.Logs) {
				if (dl.Date == CurrentDate) {
					dl.Exercises.Add (new Exercise (xName, xCalories));
					logFound = true;
				}
			}
		
			if (!logFound) {
				DailyLog log = new DailyLog();
				log.Exercises.Add (new Exercise (xName, xCalories));
				log.Date = CurrentDate;
				user.Logs.Add (log);
			}

			panelLogCardio.SetActive(false);
			UpdateDetailPanels();	
		}
	}

	public void RemoveCardioLog(Exercise exercise)
	{
		foreach (DailyLog dl in user.Logs)
		{
			if(dl.Date == CurrentDate)
			{
				foreach (Exercise ex in dl.Exercises)
				{
					if (ex == exercise)
					{
						dl.Exercises.Remove(ex);
						UpdateDetailPanels();
						return;
					}
				}
			}
		}
	}

	public void AddDietLog()
	{
		string xName;
		int xCalories;
		
		if (textDietNameLog.text != "" && textDietCaloriesLog.text != "") {
			xName = textDietNameLog.text;
			xCalories = Convert.ToInt32 (textDietCaloriesLog.text);
		
		
			bool logFound = false;
		
			foreach (DailyLog dl in user.Logs) {
				if (dl.Date == CurrentDate) {
					dl.Meals.Add (new Meal (xName, xCalories));
					logFound = true;
				}
			}
		
			if (!logFound) {
				DailyLog log = new DailyLog();
				log.Meals.Add (new Meal (xName, xCalories));
				log.Date = CurrentDate;
				user.Logs.Add (log);
			}

			panelLogDiet.SetActive(false);
			UpdateDetailPanels();
		}
	}

	public void RemoveDietLog(Meal meal)
	{
		foreach (DailyLog dl in user.Logs)
		{
			if(dl.Date == CurrentDate)
			{
				foreach (Meal ml in dl.Meals)
				{
					if (ml == meal)
					{
						dl.Meals.Remove(ml);
						UpdateDetailPanels();
						return;
					}
				}
			}
		}
	}


	public void AddWaterLog(int quantity)
	{	
		bool logFound = false;
	
		foreach (DailyLog dl in user.Logs) {
			if (dl.Date == CurrentDate) {
				dl.WaterConsumed += quantity;
				logFound = true;
			}
		}
	
		if (!logFound) {
			DailyLog log = new DailyLog ();
			log.WaterConsumed = quantity;
			log.Date = CurrentDate;
			user.Logs.Add (log);
		}

		UpdateDetailPanels();
	}

	public void ChangeTab(GameObject tabsPanel)
	{
		panelBmi.SetActive (false);
		panelSettings.SetActive (false);
		panelDiet.SetActive (false);
		panelWater.SetActive (false);

		tabsPanel.SetActive (true);

		if (tabsPanel == panelSettings)
			GetComponent<SettingsController> ().UpdatePlaceHolders (user);
	}

	public void DEBUGChangeTab()
	{
		panelBmi.SetActive (true);
		panelSettings.SetActive (false);
		panelDiet.SetActive (false);
		panelWater.SetActive (false);
	}

	public void OpenNewLog(GameObject logPanelToOpen)
	{
		logPanelToOpen.SetActive (true);
	}

	public void CancelLog(GameObject logPanelToClose)
	{
		logPanelToClose.SetActive (false);
	}
}
