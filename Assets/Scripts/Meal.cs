using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Meal
{
	public string Name;
	public int Calories;

	public Meal(string name, int calories)
	{
		Name = name;
		Calories = calories;

	}
}
