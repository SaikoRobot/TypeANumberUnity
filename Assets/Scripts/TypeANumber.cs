using UnityEngine;
using System.Collections;

public class TypeANumber : MonoBehaviour {
	
	
	public string achievementKeyPrime = "";
	public string achievementKeyHumble = "";
	public string achievementKeyLeet = "";
	public string achievementKeyArrogant = "";
	public string achievementKeyBored = "";
	public string achievementKeyReallyBored = "";
	public string leaderboardKeyEasy = "";
	public string leaderboardKeyHard = "";
	
	class AccomplishmentsOutbox {
		public bool prime;
		public bool humble;
		public bool leet;
		public bool arrogant;
		public int bored;
		public int easyScore;
		public int hardScore;

		public AccomplishmentsOutbox(){
			prime = false;
			humble = false;
			leet = false;
			arrogant = false;
			bored = 0;
			easyScore = -1;
			hardScore = -1;
		}
	}
	
	enum Mode {
		Menu,
		Game,
		Win
	}
	private bool signedin = false;
	
	private int digit = 0;
	private int score = 0;
	private bool easy = false;
	private Mode mode = Mode.Menu;
	private AccomplishmentsOutbox outbox = new AccomplishmentsOutbox();
	private GooglePlayGamesService gpgs;
	private string displayName;
		
	GUIStyle headerStyle = new GUIStyle();
	GUIStyle commonStyle = new GUIStyle();
	
	void Start () {
		headerStyle.normal.textColor = Color.white;
		headerStyle.richText = true;
		commonStyle.normal.textColor = Color.white;
		commonStyle.wordWrap = true;
		commonStyle.richText = true;
		
		gpgs = GameObject.FindObjectOfType(typeof(GooglePlayGamesService)) as GooglePlayGamesService;
		gpgs.SetCallbackObject(this.gameObject);
		signedin = gpgs.IsSignedIn();
	}
	
	void Login() {
		if (signedin)
			return;
		gpgs.SignIn();
	}
	
	void Logout() {
		if (!signedin)
			return;
		gpgs.SignOut();
		signedin = false;
		displayName = "";
	}
	
	void ShowLeaderboards() {
		if (!signedin)
			return;
		gpgs.ShowLeaderboards();
	}
	
	void ShowAchievements() {
		if (!signedin)
			return;
		gpgs.ShowAchievements();
	}
	
	void OnSignInSucceeded() {
		signedin = true;
		PushAccomplishments();
		displayName = gpgs.GetDisplayName();
	}
	
	void OnSignInFailed() {
		signedin = false;
		displayName = "";
	}
	
	string GetDigit() {
		return string.Format("{0:0000}", digit);
	}
	
	void OnClickDigit(int num) {
		digit = digit * 10 + num;
		digit = digit % 10000;
	}
	
	void CalcScore() {
		if (easy) {
			score = digit;
		} else {
			score = digit / 2;
		}
		
		CheckForAchievements();
		
		if (easy && outbox.easyScore < score) {
			outbox.easyScore = score;
		} else if (!easy && outbox.hardScore < score) {
			outbox.hardScore = score;
		}
		
		PushAccomplishments();
	}
	
	bool IsPrime(int num) {
		if (num == 0 || num == 1) return false;
		for (int i = 2; i <= num / 2; i++) {
			if (num % i == 0)
				return false;
		}
		return true;
	}
	
	void CheckForAchievements() {
		if (IsPrime(score))
			outbox.prime = true;
		if (digit == 9999)
			outbox.arrogant = true;
		if (digit == 0)
			outbox.humble = true;
		if (digit == 1337)
			outbox.leet = true;
		
		outbox.bored++;
	}
	
	void PushAccomplishments() {
		if (!gpgs.IsSignedIn())
			return;
		
		if (outbox.prime) {
			gpgs.UnlockAchievement(achievementKeyPrime);
			outbox.prime = false;
		}
		if (outbox.arrogant) {
			gpgs.UnlockAchievement(achievementKeyArrogant);
			outbox.arrogant = false;
		}
		if (outbox.humble) {
			gpgs.UnlockAchievement(achievementKeyHumble);
			outbox.humble = false;
		}
		if (outbox.leet) {
			gpgs.UnlockAchievement(achievementKeyLeet);
			outbox.leet = false;
		}
		if (outbox.bored > 0) {
			gpgs.IncrementAchievement(achievementKeyBored, outbox.bored);
			gpgs.IncrementAchievement(achievementKeyReallyBored, outbox.bored);
			outbox.bored = -1;
		}
		if (outbox.easyScore >= 0) {
			gpgs.SubmitScore(leaderboardKeyEasy, outbox.easyScore);
			outbox.easyScore = -1;
		}
		if (outbox.hardScore >= 0) {
			gpgs.SubmitScore(leaderboardKeyHard, outbox.hardScore);
			outbox.hardScore = -1;
		}
	}
	
	
	void OnGUI () {
		GUIScale();
		GUIStyle buttonStyle = GUI.skin.GetStyle("Button");
		buttonStyle.fontSize = 24;
		buttonStyle.padding  = new RectOffset(10, 10, 10, 10);

		switch(mode) {
		case Mode.Menu:
			DrawHeader();
			DrawMenu();
			DrawFooter();
			break;
		case Mode.Game:
			DrawGame();
			break;
		case Mode.Win:
			DrawWin();
			DrawFooter();
			break;
		}
	}
	
	void GUIScale() {
		float sX = (float)Screen.width / 480F;
		float asp = (float)Screen.height / (float)Screen.width;
		float sY = sX * asp;
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(sX, sY, 1));
	}

	void DrawHeader () {
		GUILayout.BeginArea(new Rect(70, 20, 340, 40));
		GUILayout.Label("<size=28><color=red>Type-a-Number Challenge</color></size>", headerStyle);
		GUILayout.EndArea();
	}
	
	
	void DrawMenu() {
		GUILayout.BeginArea(new Rect(70, 60, 340, 300));
		GUILayout.BeginVertical();
		
		string text =  (signedin) ? "Hello, " + displayName : "signin!";
		GUILayout.Label("<size=20><color=white>"+text+"</color></size>", commonStyle);
		
		GUILayout.FlexibleSpace();
		
		GUILayout.Label("<size=20><color=white>Choose a difficulty to play:</color></size>", commonStyle);
		
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("easy")) {
			easy = true;
			mode = Mode.Game;
		}
		if (GUILayout.Button("hard")) {
			easy = false;
			mode = Mode.Game;
		}
		GUILayout.EndHorizontal();

		GUILayout.FlexibleSpace();
		
		GUILayout.Label("<size=20><color=white>See your progress:</color></size>", commonStyle);
		
		if (GUILayout.Button("Show Achievements")){
			ShowAchievements();
		}
		if (GUILayout.Button("Show Leaderboards")){
			ShowLeaderboards();
		}
		GUILayout.FlexibleSpace();
		
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}
	
	void DrawGame() {
		GUILayout.BeginArea(new Rect(70, 60, 340, 400));
		GUILayout.BeginVertical();

		GUILayout.Label("<size=20><color=white>What score do you think you deserve?</color></size>", commonStyle);
		
		GUILayout.FlexibleSpace();
		
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.Label("<size=30><color=white>"+ GetDigit() +"</color></size>", commonStyle);
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("7")) {
			OnClickDigit(7);
		}
		if (GUILayout.Button("8")) {
			OnClickDigit(8);
		}
		if (GUILayout.Button("9")) {
			OnClickDigit(9);
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("4")) {
			OnClickDigit(4);
		}
		if (GUILayout.Button("5")) {
			OnClickDigit(5);
		}
		if (GUILayout.Button("6")) {
			OnClickDigit(6);
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("1")) {
			OnClickDigit(1);
		}
		if (GUILayout.Button("2")) {
			OnClickDigit(2);
		}
		if (GUILayout.Button("3")) {
			OnClickDigit(3);
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("0")) {
			OnClickDigit(0);
		}
		if (GUILayout.Button("Clear")) {
			OnClickDigit(0);
			OnClickDigit(0);
			OnClickDigit(0);
			OnClickDigit(0);
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		
		if (GUILayout.Button("OK")){
			CalcScore();
			mode = Mode.Win;
			
		}
		
		GUILayout.FlexibleSpace();

		GUILayout.EndVertical();
		GUILayout.EndArea();
	}

	void DrawWin() {
		GUILayout.BeginArea(new Rect(70, 60, 340, 300));
		GUILayout.BeginVertical();

		GUILayout.Label("<size=20><color=white>You win? Your score is:</color></size>", commonStyle);

		GUILayout.FlexibleSpace();
		
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.Label("<size=30><color=white>"+ score +"</color></size>", commonStyle);
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		
		GUILayout.FlexibleSpace();
		
		string text1 = (easy) ? "easy" : "hard";
		string text2 = (easy) ? "so you got the score you requested! Wasn't that easy?" 
			: "so you only got half the score you requested! Isn't this game hard?";
		GUILayout.Label("<size=20><color=white>You're playing in "+text1+" mode. "+text2+"</color></size>", commonStyle);
		
		if (GUILayout.Button("OK")){
			mode = Mode.Menu;
		}
		
		GUILayout.FlexibleSpace();
		
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}
	
	void DrawFooter() {
		GUILayout.BeginArea(new Rect(70, 380, 340, 50));
		GUILayout.BeginHorizontal();

		if (!signedin && (mode == Mode.Menu || mode == Mode.Win)) {
			if (GUILayout.Button("g+ login")){
				Login();
			}
		}

		string text = (signedin) ? "You are signed in whith Google" 
			: "Sign in with Google to share your scores and achievements with your firends.";
		
		GUILayout.Label("<size=14><color=white>"+text+"</color></size>", commonStyle);

		if (signedin && mode == Mode.Menu) {
			if (GUILayout.Button("logout")){
				Logout();
			}
		}
		
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}
}
