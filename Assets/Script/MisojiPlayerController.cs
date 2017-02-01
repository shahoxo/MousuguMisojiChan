using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UniRx;

public class MisojiPlayerController : NetworkBehaviour {
    [SyncVar]
    public MisojiChanGame.Player player;
    private AgeSelector ageSelector;

    private MisojiChanClientToServerMessenger messenger = new MisojiChanClientToServerMessenger();



    public void SetPlayer(MisojiChanGame.Player player)
    {
        this.player = player;
    }

    public void SetAgeSelector(AgeSelector ageSelector)
    {
        this.ageSelector = ageSelector;

        this.ageSelector.ClickAsObservable.Subscribe(age =>
        {
            messenger.SendAge(player.id, age);
        }).AddTo(this);
    }

}
