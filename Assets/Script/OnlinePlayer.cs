using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class OnlinePlayer : NetworkBehaviour {
	
	public bool isIncremented = false;
	public bool isMyTurn = true;
	public int incrementedAge = 0;
	public Button buttonOne;
	public Button buttonTwo;
	public Button buttonThree;
	public MisojiChanGame.Player player;
	void Update () {
		if(!isLocalPlayer)
			return;
		if(Input.GetKeyDown("1"))
			IncrementAge(1);
		if(Input.GetKeyDown("2"))
			IncrementAge(2);
		if(Input.GetKeyDown("3"))
			IncrementAge(3);
	}

	void IncrementAge(int year) {
		isIncremented = true;
		DisableInputs();
		CmdIncrementAge(year);
	}

	void DisableInputs() {
		
	}

	[Command]
	void CmdIncrementAge(int year) {
		incrementedAge = year;
	}
}
