﻿using System.Collections;
using System.Collections.Generic;
using UniRx;

public class MisojiChanGame {
	//public int age = 0;
    public ReactiveProperty<int> age = new ReactiveProperty<int>(0);
	public int turn = 1;
	public Player firstPlayer;
	public Player secondPlayer;
	public Player winner;
	public Player turnPlayer;

	public MisojiChanGame(string firstPlayerName = "1P", string secondPlayerName = "2P") {
		firstPlayer = new Player() { id = 1, name = firstPlayerName };
		secondPlayer = new Player() { id = 2, name = secondPlayerName };
		UpdateTurnPlayer();
	}

	public void IncrementAgeByFirstPlayer(int age) {
		IncrementAge(firstPlayer, age);
	}
	public void IncrementAgeBySecondPlayer(int age) {
		IncrementAge(secondPlayer, age);
	}

	public void Reset() {
		age.Value = 0;
		winner = null;
	}
	void IncrementAge(Player player, int age) {
		if(IsEnding())
			return;
        this.age.Value += age;
		ProgressTurn();
		if(IsEnding())
			winner = FindWinner(player);
	}

	public bool IsEnding() {
		return age.Value >= 30;
	}

	Player FindWinner(Player player) {
        return firstPlayer == player ? secondPlayer : firstPlayer;
	}

	void ProgressTurn() {
		turn++;
		UpdateTurnPlayer();
	}

	Player FindTurnPlayer() {
		return turn % 2 == 1 ? firstPlayer : secondPlayer;
	}

	void UpdateTurnPlayer() {
		turnPlayer = FindTurnPlayer();
	}

    [System.Serializable]
	public class Player  {
        public int id;
        public string name;
	}

    [System.Serializable]
    public class Turn
    {
        public int id;
        public int age;
    }

}
