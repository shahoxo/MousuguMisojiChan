using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightController : MonoBehaviour {
	public Text textComponent;
	public Text firstPlayerResult;
	public Text secondPlayerResult;
	public Button retryButton;
	public Text turnPlayer;
	
	MisojiChanGame game = new MisojiChanGame();

	void Awake() {
		turnPlayer.text = game.turnPlayer.name;
	}

	void Update() {
		WatchEnd();
		textComponent.text = game.age.ToString();
		turnPlayer.text = game.turnPlayer.name;
		WatchEnd();
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
