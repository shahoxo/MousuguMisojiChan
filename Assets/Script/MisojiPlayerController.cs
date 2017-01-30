using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MisojiPlayerController : NetworkBehaviour {
    public MisojiChanGame.Player player;
    [SyncVar]
    public bool isMyTurn;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
