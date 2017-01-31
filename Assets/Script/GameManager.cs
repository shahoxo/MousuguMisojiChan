using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UniRx;

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
    MisojiChanMessenger messenger;

    static public void AddPlayer(GameObject gamePlayer, string _name, short playerControllerId)
    {
        var commer = new MisojiChanGame.Player() { name = _name };
        gamePlayer.GetComponent<MisojiPlayerController>().player = commer;
        gamePlayer.name = commer.name;
        players.Add(gamePlayer.GetComponent<MisojiPlayerController>());
        Debug.Log(players.Count);
    }

    void Awake()
    {
        messenger = new MisojiChanMessenger();
        NetworkServer.RegisterHandler(MisojiChanMessageType.SendAge, OnSendAge);
        NetworkManager.singleton.client.RegisterHandler(MisojiChanMessageType.SendTurnPlayer, OnSnedTurnPlayer);
        retryButton.OnClickAsObservable().Subscribe(_ =>
        {
            this.RetryFight();
        }).AddTo(this);

        ageSelector.ClickAsObservable.Subscribe(age =>
        {
            Debug.Log("age: " + age);
            messenger.SendAge(age);
        }).AddTo(this);

    }

    [Server]
    void OnSendAge(NetworkMessage message)
    {
        var misojichanMessage = message.ReadMessage<MisojiChanMessenger.MisojiChanMessage>();
        this.IncrementAge(misojichanMessage.age);
    }
    [Client]
    void OnSnedTurnPlayer(NetworkMessage message)
    {
        var turnPlayerMessage = message.ReadMessage<MisojiChanMessenger.TurnPlayerMessage>();

        turnPlayerText.text = turnPlayerMessage.name;

    }

    [ServerCallback]
    IEnumerator Start()
    {
        while (players.Count != 2)
            yield return null;
        yield return new WaitForSeconds(2.0f);

        game = new MisojiChanGame(players[0].player.name, players[1].player.name);

        game.age.Subscribe(age =>
        {
            RpcUpdateAgeText(age);
        }).AddTo(this);
        turnPlayer = game.turnPlayer;

        while (!game.IsEnding())
        {
            messenger.SendTurnPlayer(turnPlayer);
            // 入力

            // 受け付けてインクリメントします

            // 終わってなければ手番を変えます


            yield return null;
        }
    }

    [ClientRpc]
    public void RpcUpdateAgeText(int age)
    {
        textComponent.text = age.ToString();
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
    void Update()
    {

    }

    [Server]
    public void IncrementAge(int year)
    {
        game.IncrementAgeByFirstPlayer(year);

        players[0].isMyTurn = players[0].player == game.turnPlayer;
        players[1].isMyTurn = players[1].player == game.turnPlayer;
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



public class MisojiChanMessenger
{
    public class MisojiChanMessage : MessageBase
    {
        public int age;
    }
    public class TurnPlayerMessage : MessageBase
    {
        public int id;
        public string name;
    }

    public void SendAge(int age)
    {
        MisojiChanMessage msg = new MisojiChanMessage();
        msg.age = age;

        NetworkManager.singleton.client.Send(MisojiChanMessageType.SendAge, msg);
    }

    public void SendTurnPlayer(MisojiChanGame.Player player)
    {
        TurnPlayerMessage msg = new TurnPlayerMessage();
        msg.id = player.id;
        msg.name = player.name;

        NetworkServer.SendToAll(MisojiChanMessageType.SendTurnPlayer, msg);
    }

}

public static class MisojiChanMessageType
{
    public static readonly short SendAge = 1001;

    public static readonly short SendTurnPlayer = 1002;
}