#if UNITY_ANDROID
using UnityEngine;
using System.Collections;

public class GooglePlayGamesService : MonoBehaviour {
	
	GameObject callbackGameObject;
	
    static AndroidJavaClass pluginClass;
    static AndroidJavaClass unityPlayer;
    static AndroidJavaObject currActivity;
	
	void Start () {
		pluginClass =  new AndroidJavaClass("com.saikorobot.gms.Plugin");
		unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		currActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
		pluginClass.CallStatic("start", gameObject.name);
		Debug.Log("GooglePlayGamesService Start plugin.start gameObject.name=" + gameObject.name);
	}

	public void SetCallbackObject(GameObject callbackGameObject) {
		this.callbackGameObject = callbackGameObject;
		Debug.Log("GooglePlayGamesService SetCallbackObject callbackObject.name="+callbackGameObject.name);
	}
	
	public void SignIn() {
		pluginClass.CallStatic("signin", currActivity);
		Debug.Log("GooglePlayGamesService SignIn");
	}
	
	public void SignOut() {
		pluginClass.CallStatic("signout", currActivity);
		Debug.Log("GooglePlayGamesService SignOut");
	}
	
	public bool IsSignedIn() {
		bool result =  pluginClass.CallStatic<bool>("isSignedIn", currActivity);
		Debug.Log("GooglePlayGamesService IsSignedIn =" + result);
		return result;
	}
	
	public void ShowAchievements() {
		pluginClass.CallStatic("showAchievements", currActivity);
		Debug.Log("GooglePlayGamesService ShowAchievements");
	}
	
	public void ShowLeaderboards() {
		pluginClass.CallStatic("showLeaderboards", currActivity);
		Debug.Log("GooglePlayGamesService ShowLeaderboards");
	}
	
	public void UnlockAchievement(string id) {
		pluginClass.CallStatic("unlockAchievement", currActivity, id);
		Debug.Log("GooglePlayGamesService UnlockAchievement id=" + id.Substring(id.Length-4, 3));
	}
	
	public void IncrementAchievement(string id, int num) {
		pluginClass.CallStatic("incrementAchievement", currActivity, id, num);
		Debug.Log("GooglePlayGamesService IncrementAchievement id=" + id.Substring(id.Length-4, 3) + " num="+num);
	} 
	
	public void SubmitScore(string id, int score) {
		pluginClass.CallStatic("submitScore", currActivity, id, score);
		Debug.Log("GooglePlayGamesService SubmitScore id=" + id.Substring(id.Length-4, 3) + " score="+score);
	}
	
	public string GetDisplayName() {
		string name = pluginClass.CallStatic<string>("getDisplayName", currActivity);
		Debug.Log("GooglePlayGamesService GetDisplayName =" + name);
		return name;
	}
	
	// -- callback
	void OnSignInSucceeded(string msg) {
		Debug.Log("GooglePlayGamesService OnSignInSucceeded");
		if (callbackGameObject != null)
			callbackGameObject.SendMessage("OnSignInSucceeded", SendMessageOptions.DontRequireReceiver);
	}
	void OnSignInFailed(string msg) {
		Debug.Log("GooglePlayGamesService OnSignInFailed");
		if (callbackGameObject != null)
			callbackGameObject.SendMessage("OnSignInFailed", SendMessageOptions.DontRequireReceiver);
	}
	
}
#endif