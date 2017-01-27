using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour {

	static public List<MisojiChanGame.Player> players = new List<MisojiChanGame.Player>();
	public Text textComponent;
	public Text firstPlayerResult;
	public Text secondPlayerResult;
	public Button retryButton;
	public Text turnPlayer;
	MisojiChanGame game;
	
	static public void AddPlayer(GameObject gamePlayer, string _name, short playerControllerId) {
		var commer = new MisojiChanGame.Player() { name = _name };
		players.Add(commer);
		Debug.Log(players.Count);
	}

	[ServerCallback]
	private void Start(){
		if(players.Count != 2)
			Debug.Log("Not supported");
		game = new MisojiChanGame(players[0].name, players[2].name);
		turnPlayer.text = game.turnPlayer.name;
	}

/*
	void Update() {
		WatchEnd();
		textComponent.text = game.age.ToString();
		turnPlayer.text = game.turnPlayer.name;
		WatchEnd();
	}
	*/

	[ServerCallback]
	void Update() {
		
	}

	public void IncrementAge(int year) {
		game.IncrementAgeByFirstPlayer(year);
	}

	void WatchEnd() {
		if(game.IsEnding()) {
			ShowResult();
		}
	}

	void ShowResult() {
		var result = game.winner == game.firstPlayer ? new string[] { "Win", "Lose" } : new string[] { "Lose", "Win" };
		firstPlayerResult.text = result[0];
		secondPlayerResult.text = result[1];
		retryButton.gameObject.SetActive(true);
	}

	public void RetryFight() {
		game.Reset();
		firstPlayerResult.text = secondPlayerResult.text = "";
		retryButton.gameObject.SetActive(false);
	}
}
