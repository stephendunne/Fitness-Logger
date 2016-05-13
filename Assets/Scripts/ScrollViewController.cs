using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ScrollViewController : MonoBehaviour {

	public GameObject exerciseObjectTemplate;
	public Transform exercisesListParent;

	public GameObject mealObjectTemplate;
	public Transform mealsListParent;

	public MainController mainController;

	void Start()
	{
		mainController = GetComponent<MainController> ();
	}

	// Use this for initialization
	public int UpdateExerciseScrollViewPanel(DailyLog log) {

		foreach (Transform child in exercisesListParent) 
			if(child.tag == "Exercise")
				Destroy(child.gameObject);

		int exerciseCaloriesTotal = 0;
		
		foreach(Exercise ex in log.Exercises)
		{
			GameObject go = Instantiate(exerciseObjectTemplate) as GameObject;
			go.SetActive(true);
			go.GetComponentInChildren<Text>().text = ex.Name + ": " + ex.CaloriesBurned + " calories burned";
			go.GetComponentInChildren<Button>().onClick.AddListener(delegate {mainController.RemoveCardioLog(ex);});
			go.transform.SetParent(exercisesListParent);
			exerciseCaloriesTotal += ex.CaloriesBurned;
		}

		return exerciseCaloriesTotal;
	}

	public int UpdateMealsScrollViewPanel(DailyLog log) {
		foreach (Transform child in mealsListParent)
			if(child.tag == "Meal")
				Destroy(child.gameObject);

		int mealCaloriesTotal = 0;
		
		foreach(Meal m in log.Meals)
		{
			GameObject go = Instantiate(mealObjectTemplate) as GameObject;
			go.SetActive(true);
			go.GetComponentInChildren<Text>().text = m.Name + ": " + m.Calories + " calories";
			go.GetComponentInChildren<Button>().onClick.AddListener(delegate {mainController.RemoveDietLog(m);});
			go.transform.SetParent(mealsListParent);
			mealCaloriesTotal += m.Calories;
		}

		return mealCaloriesTotal;
	}

	public void ClearScrollViewPanels()
	{
		foreach (Transform child in exercisesListParent) 
			if(child.tag == "Exercise")
				Destroy(child.gameObject);

		foreach (Transform child in mealsListParent)
			if(child.tag == "Meal")
				Destroy(child.gameObject);
	}
}
