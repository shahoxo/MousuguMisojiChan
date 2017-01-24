using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightController : MonoBehaviour {
	public int age = 0;

	void Awake() {
		age = 0;
	}

	void Update() {
		
	}

	public void IncrementAge(int year) {
		age += year;
		Debug.Log(age);
	}
}
