using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour {

	static public List<MisojiChanGame.Player> players = new List<MisojiChanGame.Player>();
	
	static public void AddPlayer(GameObject gamePlayer, string _name, short playerControllerId) {
		var commer = new MisojiChanGame.Player() { name = _name };
		players.Add(commer);
	}
}
