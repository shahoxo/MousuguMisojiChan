using System.Collections;
using System.Collections.Generic;

public class MisojiChanGame {
	public int age = 0;
	public int turn = 1;
	public Player firstPlayer;
	public Player secondPlayer;
	public Player winner;
	public Player turnPlayer;

	public MisojiChanGame(string firstPlayerName = "1P", string secondPlayerName = "2P") {
		firstPlayer = new Player() { name = firstPlayerName };
		secondPlayer = new Player() { name = secondPlayerName };
		UpdateTurnPlayer();
	}

	public void IncrementAgeByFirstPlayer(int age) {
		IncrementAge(firstPlayer, age);
	}
	public void IncrementAgeBySecondPlayer(int age) {
		IncrementAge(secondPlayer, age);
	}

	public void Reset() {
		age = 0;
		winner = null;
	}
	void IncrementAge(Player player, int age) {
		if(IsEnding())
			return;
		this.age += age;
		ProgressTurn();
		if(IsEnding())
			winner = FindWinner(player);
	}

	public bool IsEnding() {
		return age >= 30;
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

	public class Player  {
		public string name;
	}
}
