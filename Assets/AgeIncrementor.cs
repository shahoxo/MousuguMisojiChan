using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void AgeHandler(int year);
public class AgeIncrementor : MonoBehaviour {
	
	public event AgeHandler OnIncrementAge;

	void Start() {
		//OnIncrementAge += GetComponent<FightController>().IncrementAge;
	}
	public void IncrementAge(int year) {
		// fire incrementing
		Debug.Log(year);
		//OnIncrementAge(year);
	}

	
}
