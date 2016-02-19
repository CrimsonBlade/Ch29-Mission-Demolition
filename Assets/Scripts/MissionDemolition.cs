using UnityEngine;
using System.Collections;

public enum GameMode{
	idle,
	playing,
	levelEnd
}

public class MissionDemolition : MonoBehaviour {
	static public MissionDemolition S;

	//Fields to be set in unity
	public GameObject[] castles; //array of castles
	public GUIText gtLevel; //the GT_Level GUIText
	public GUIText gtScore; 
	public Vector3 castlePos; // the place to put the castle

	public bool ___________________;
	//fields set dynamiclly
	public int level;
	public int levelMax;
	public int shotsTaken;
	public GameObject castle;
	public GameMode mode = GameMode.idle;
	public string showing = "Slingshot";// follow cam mode

	// Use this for initialization
	void Start () {
		S = this;
		level = 0;
		levelMax = castles.Length;
		StartLevel ();
	
	}

	void StartLevel()
	{
		if (castle != null) {
			Destroy(castle);
		}

		//destroy old projectiles
		GameObject[] gos = GameObject.FindGameObjectsWithTag ("Projectile");
		foreach (GameObject pTemp in gos) {
			Destroy (pTemp);
		}

		//Instantiate new castle
		castle = Instantiate (castles [level]) as GameObject;
		castle.transform.position = castlePos;
		shotsTaken = 0;

		//reset camera
		SwitchView("Both");
		ProjectileLine.S.Clear ();

		//reset the goal
		Goal.goalMet = false;

		ShowGT ();

		mode = GameMode.playing;
	}

	void ShowGT()
	{
		gtLevel.text = "Level1: " + (level + 1) + " of " + levelMax;
		gtScore.text = "Shots Taken: " + shotsTaken;
	}
	
	// Update is called once per frame
	void Update () {
		ShowGT ();
		if (mode == GameMode.playing && Goal.goalMet) {
			mode = GameMode.levelEnd;
			//ZOOM OUT
			SwitchView("Both");
			Invoke ("NextLevel", 2f);
		}
	}

	void NextLevel ()
	{
		level++;
		if (level == levelMax) {
			level = 0;
		}
		StartLevel ();
	}

	void OnGUI()
	{
		Rect buttonRect = new Rect ((Screen.width / 2) - 50, 10, 100, 24);

		switch (showing) {
		case "Slingshot":
			if (GUI.Button(buttonRect, "Show Castle"))
			{
				SwitchView("Castle");
			}
			break;

		case "Castle":
			if(GUI.Button(buttonRect, "Show Both"))
			{
				SwitchView("Both");
			}
			break;

		case "Both":
			if (GUI.Button(buttonRect, "Show Slingshot"))
			{
				SwitchView("Slingshot");
			}
			break;
		}
	}

	static public void SwitchView(string eView)
	{
		S.showing = eView;
		switch (S.showing) {
		case "Slingshot":
			FollowCam.S.poi = null;
			break;

		case "Casstle":
			FollowCam.S.poi = S.castle;
			break;

		case "Both":
			FollowCam.S.poi = GameObject.Find ("ViewBoth");
			break;
		}
	}

	public static void ShotFired()
	{
		S.shotsTaken++;
	}
}
