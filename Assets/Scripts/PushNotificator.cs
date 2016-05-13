using UnityEngine;
using System.Collections;

public class PushNotificator : MonoBehaviour {
	// Use this for initialization
	void Start () {
		Pushwoosh.ApplicationCode = "ENTER_PUSHWOOSH_APP_ID_HERE";
		Pushwoosh.GcmProjectNumber = "ENTER_GOOGLE_PROJECT_NUMBER_HERE";
		Pushwoosh.Instance.OnRegisteredForPushNotifications += OnRegisteredForPushNotifications;
		Pushwoosh.Instance.OnFailedToRegisteredForPushNotifications += OnFailedToRegisteredForPushNotifications;
		Pushwoosh.Instance.OnPushNotificationsReceived += OnPushNotificationsReceived;
		Pushwoosh.Instance.RegisterForPushNotifications ();
	}
	
	void OnRegisteredForPushNotifications(string token)
	{
		//do handling here
		Debug.Log("Received token: \n" + token);
	}
	
	void OnFailedToRegisteredForPushNotifications(string error)
	{
		//do handling here
		Debug.Log("Error ocurred while registering to push notifications: \n" + error);
	}
	
	void OnPushNotificationsReceived(string payload)
	{
		//do handling here
		Debug.Log("Received push notificaiton: \n" + payload);
	}
}