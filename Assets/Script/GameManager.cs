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
	MisojiChanGame gameModel;
	[SyncVar]
	public int age = 0;
	
	static public void AddPlayer(GameObject gamePlayer, string _name, short playerControllerId) {
		var commer = new MisojiChanGame.Player() { name = _name };
		players.Add(commer);
		Debug.Log(players.Count);
	}

	[ServerCallback]
	private void Start(){
		if(players.Count != 2)
			Debug.Log("Not supported");
		gameModel = new MisojiChanGame(players[0].name, players[2].name);
	}

	void Update(){
		age = gameModel.age;
	}
}
