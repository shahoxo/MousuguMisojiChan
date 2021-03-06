﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UniRx;
using System.Linq;

public class GameManager : NetworkBehaviour
{

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
    [SerializeField]
    AgeSelector ageSelector;
    MisojiChanClientToServerMessenger clientToServerMessenger;
    MisojiChanServerToClientMessenger serverToClientMessenger;

    MisojiPlayerController currentClientPlayer;
    MisojiChanClientToServerMessenger.SelectedAgeMessage receivedAgeMessage;

    static public void AddPlayer(GameObject gamePlayer, string _name, short playerControllerId)
    {
        var commer = new MisojiChanGame.Player() { name = _name };
        gamePlayer.GetComponent<MisojiPlayerController>().SetPlayer(commer);
        gamePlayer.name = commer.name;
        players.Add(gamePlayer.GetComponent<MisojiPlayerController>());

        Debug.Log(players.Count);
    }

    void Awake()
    {
        clientToServerMessenger = new MisojiChanClientToServerMessenger();
        serverToClientMessenger = new MisojiChanServerToClientMessenger();
        NetworkServer.RegisterHandler(MisojiChanMessageType.SendAge, OnReceivedAge);
        NetworkManager.singleton.client.RegisterHandler(MisojiChanMessageType.SendTurnPlayer, OnSnedTurnPlayer);
        NetworkManager.singleton.client.RegisterHandler(MisojiChanMessageType.InitializedGame, OnInitializeGame);
        NetworkManager.singleton.client.RegisterHandler(MisojiChanMessageType.SendWinner, OnReceivedWinner);
        retryButton.OnClickAsObservable().Subscribe(_ =>
        {
            this.RetryFight();
        }).AddTo(this);

    }

    [Server]
    void OnReceivedAge(NetworkMessage message)
    {
        var ageMessage = message.ReadMessage<MisojiChanClientToServerMessenger.SelectedAgeMessage>();
        Debug.Log("id: " + ageMessage.id + ", age: " + ageMessage.age);
        receivedAgeMessage = ageMessage;
    }
    [Client]
    void OnSnedTurnPlayer(NetworkMessage message)
    {
        var turnPlayerMessage = message.ReadMessage<MisojiChanServerToClientMessenger.TurnPlayerMessage>();
        currentClientPlayer = GameObject.FindObjectsOfType<MisojiPlayerController>().FirstOrDefault(player => player.isLocalPlayer);
        turnPlayerText.text = (currentClientPlayer.player.id == turnPlayerMessage.id ? "あなたの手番です" : turnPlayerMessage.name + "の手番です");

    }
    [Client]
    void OnInitializeGame(NetworkMessage message)
    {
        currentClientPlayer = GameObject.FindObjectsOfType<MisojiPlayerController>().FirstOrDefault(player => player.isLocalPlayer);
        currentClientPlayer.SetAgeSelector(ageSelector);
    }
    [Client]
    void OnReceivedWinner(NetworkMessage message)
    {
        var winnerMessage = message.ReadMessage<MisojiChanServerToClientMessenger.WinnerMessage>();
        turnPlayerText.text = winnerMessage.id == currentClientPlayer.player.id ? "あなたの勝ち" : "あなたの負け";
    }


    [ServerCallback]
    IEnumerator Start()
    {
        while (players.Count != 2)
            yield return null;
        yield return new WaitForSeconds(1.0f);

        game = new MisojiChanGame(players[0].player.name, players[1].player.name);
        players[0].player = game.firstPlayer;
        players[1].player = game.secondPlayer;

        serverToClientMessenger.InitializedGame();

        game.age.Subscribe(age =>
        {
            RpcUpdateAgeText(age);
        }).AddTo(this);

        while (!game.IsEnding())
        {
            turnPlayer = game.turnPlayer;
            serverToClientMessenger.SendTurnPlayer(turnPlayer);
            // 入力
            while (receivedAgeMessage == null || receivedAgeMessage.id != turnPlayer.id)
                yield return null;


            this.IncrementAge(receivedAgeMessage.age);
            receivedAgeMessage = null;
            yield return null;
        }

        serverToClientMessenger.SendWinner(game.winner);
        // 終わった。

    }

    [ClientRpc]
    public void RpcUpdateAgeText(int age)
    {
        textComponent.text = age.ToString();
    }

    [ServerCallback]
    void Update()
    {

    }

    [Server]
    public void IncrementAge(int year)
    {
        game.IncrementAgeByFirstPlayer(year);
    }

    void WatchEnd()
    {
        if (game.IsEnding())
        {
            ShowResult();
        }
    }

    void ShowResult()
    {
        var result = game.winner == game.firstPlayer ? new string[] { "Win", "Lose" } : new string[] { "Lose", "Win" };
        firstPlayerResult.text = result[0];
        secondPlayerResult.text = result[1];
        retryButton.gameObject.SetActive(true);
    }

    public void RetryFight()
    {
        game.Reset();
        firstPlayerResult.text = secondPlayerResult.text = "";
        retryButton.gameObject.SetActive(false);
    }
}



public class MisojiChanClientToServerMessenger
{
    public class SelectedAgeMessage : MessageBase
    {
        public int id;
        public int age;
    }

    public void SendAge(int id, int age)
    {
        SelectedAgeMessage msg = new SelectedAgeMessage();
        msg.age = age;
        msg.id = id;

        NetworkManager.singleton.client.Send(MisojiChanMessageType.SendAge, msg);
    }

}

public class MisojiChanServerToClientMessenger
{
    public class TurnPlayerMessage : MessageBase
    {
        public int id;
        public string name;
    }

    public class InitializeGame : MessageBase
    {
    }
    public class WinnerMessage : MessageBase
    {
        public int id;
    }

    public void InitializedGame()
    {
        var msg = new InitializeGame();
        NetworkServer.SendToAll(MisojiChanMessageType.InitializedGame, msg);
    }

    public void SendTurnPlayer(MisojiChanGame.Player player)
    {
        TurnPlayerMessage msg = new TurnPlayerMessage();
        msg.id = player.id;
        msg.name = player.name;

        NetworkServer.SendToAll(MisojiChanMessageType.SendTurnPlayer, msg);
    }

    public void SendWinner(MisojiChanGame.Player winner)
    {
        WinnerMessage msg = new WinnerMessage();
        msg.id = winner.id;

        NetworkServer.SendToAll(MisojiChanMessageType.SendWinner, msg);
    }
}

public static class MisojiChanMessageType
{
    public static readonly short SendAge = 1001;

    public static readonly short SendTurnPlayer = 1002;
    public static readonly short InitializedGame = 1003;
    public static readonly short SendWinner = 1004;
}