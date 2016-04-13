using UnityEngine;
using System.Collections;
using GooglePlayGames;

public class GooglePlayController{

	// Use this for initialization
	public static void Init () {
        PlayGamesPlatform.Activate();
        Social.ShowLeaderboardUI();
	}
}
