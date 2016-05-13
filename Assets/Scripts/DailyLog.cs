using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public class DailyLog 
{
	public DateTime Date;

	public float Weight;
	public int WaterConsumed = 0;

	public List<Meal> Meals = new List<Meal>();
	public List<Exercise> Exercises = new List<Exercise>();
}
