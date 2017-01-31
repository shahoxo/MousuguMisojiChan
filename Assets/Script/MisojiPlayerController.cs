using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MisojiPlayerController : NetworkBehaviour {
    [SyncVar]
    public MisojiChanGame.Player player;


    public void SetPlayer(MisojiChanGame.Player player)
    {
        this.player = player;
    }


}
