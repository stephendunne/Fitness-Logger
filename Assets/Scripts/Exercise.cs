using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Exercise
{
	public string Name;
	public int CaloriesBurned;
	
	public Exercise(string name, int calories)
	{
		Name = name;
		CaloriesBurned = calories;
	}
}
