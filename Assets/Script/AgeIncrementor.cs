using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void AgeHandler(int year);
public class AgeIncrementor : MonoBehaviour {
	
	public event AgeHandler OnIncrementAge;
	public FightController fightController;

	void Start() {
		OnIncrementAge += fightController.IncrementAge;
	}
	public void IncrementAge(int year) {
		OnIncrementAge(year);
	}

	
}
