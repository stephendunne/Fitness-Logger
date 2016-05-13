using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum UnitMeasurement {Imperial,Metric}
public enum Genders {Male,Female}

[Serializable]
public class User
{
	public string Username;
	public string Password;

	public UnitMeasurement MeasurementPreference;
	
	public string Realname;
	public Genders Gender;

	public float Height;
	public float StartWeight;
	public float TargetWeight;

	public int CalorieAllowance;
	public int WaterTarget = 2000;

	public List<DailyLog>Logs = new List<DailyLog>();

	public User(string username, string password, string realname, float height, float startweight, float targetweight, Genders gender, UnitMeasurement measurementPref)
	{
		Username = username;
		Password = password;
		Realname = realname;
		Height = height;
		StartWeight = startweight;
		TargetWeight = targetweight;
		MeasurementPreference = measurementPref;
		Gender = gender;

		if (gender == Genders.Male)
			CalorieAllowance = 2500;
		else
			CalorieAllowance = 2000;
	}
}
