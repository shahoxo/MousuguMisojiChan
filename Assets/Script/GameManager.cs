using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UniRx

public class GameManager : NetworkBehaviour {

	static public List<MisojiPlayerController> players = new List<MisojiPlayerController>();
	public Text textComponent;
	public Text firstPlayerResult;
	public Text secondPlayerResult;
	public Button retryButton;
	public Text turnPlayerText;
    //[SyncVar(hook = "SyncTurnPlayerName")]
    public MisojiChanGame.Player turnPlayer;
	MisojiChanGame game;
    public MisojiPlayerController myPlayer;
	
	static public void AddPlayer(GameObject gamePlayer, string _name, short playerControllerId) {
        Debug.Log("hoge: " + _name);
		var commer = new MisojiChanGame.Player() { name = _name };
        gamePlayer.GetComponent<MisojiPlayerController>().player = commer;
        gamePlayer.name = commer.name;
		players.Add(gamePlayer.GetComponent<MisojiPlayerController>());
		Debug.Log(players.Count);
	}

    void Awake()
    {
        retryButton.OnClickAsObservable().Subscribe(_ =>
        {
            this.RetryFight();
        }).AddTo(this);
    }

	[ServerCallback]
    IEnumerator Start()
    {
        while (players.Count != 2)
            yield return null;
        yield return new WaitForSeconds(2.0f);

        game = new MisojiChanGame(players[0].player.name, players[1].player.name);
        turnPlayer = game.turnPlayer;
        RpcTurnPlayerName(turnPlayer);
        players[0].isMyTurn = players[0].player == game.turnPlayer;
        players[1].isMyTurn = players[1].player == game.turnPlayer;

    }


    [ClientRpc]
    public void RpcTurnPlayerName(MisojiChanGame.Player player)
    {
        turnPlayerText.text = player.name;
    }
/*
	void Update() {
		WatchEnd();
		textComponent.text = game.age.ToString();
		turnPlayer.text = game.turnPlayer.name;
		WatchEnd();
	}
	*/

	[ServerCallback]
	void Update() {
		
	}

    [Server]
	public void IncrementAge(int year) {
		game.IncrementAgeByFirstPlayer(year);

        players[0].isMyTurn = players[0].player == game.turnPlayer;
        players[1].isMyTurn = players[1].player == game.turnPlayer;
	}

	void WatchEnd() {
		if(game.IsEnding()) {
			ShowResult();
		}
	}

	void ShowResult() {
		var result = game.winner == game.firstPlayer ? new string[] { "Win", "Lose" } : new string[] { "Lose", "Win" };
		firstPlayerResult.text = result[0];
		secondPlayerResult.text = result[1];
		retryButton.gameObject.SetActive(true);
	}

	public void RetryFight() {
		game.Reset();
		firstPlayerResult.text = secondPlayerResult.text = "";
		retryButton.gameObject.SetActive(false);
	}
}
