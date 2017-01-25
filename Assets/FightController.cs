using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightController : MonoBehaviour {
	public int age = 0;
	public Text textComponent;
	public Text firstPlayerResult;
	public Text secondPlayerResult;
	private bool isEnding = false;

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
		if(age >= 30) {
			ShowResult();
			isEnding = true;
		}
	}

	void ShowResult() {
		firstPlayerResult.text = "Win";
		secondPlayerResult.text = "Lose";
	}
}
