using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightController : MonoBehaviour {
	public int age = 0;
	public Text textComponent;
	public Text firstPlayerResult;
	public Text secondPlayerResult;
	public Button retryButton;
	private bool isEnding { get { return age >= 30;} }

	void Awake() {
		age = 0;
	}

	void Update() {
		WatchEnd();
		textComponent.text = age.ToString();
		WatchEnd();
	}

	public void IncrementAge(int year) {
		if(isEnding)
			return;
		age += year;
	}

	void WatchEnd() {
		if(isEnding) {
			ShowResult();
		}
	}

	void ShowResult() {
		firstPlayerResult.text = "Win";
		secondPlayerResult.text = "Lose";
		retryButton.gameObject.SetActive(true);
	}

	public void RetryFight() {
		age = 0;
		firstPlayerResult.text = secondPlayerResult.text = "";
		retryButton.gameObject.SetActive(false);
	}
}
