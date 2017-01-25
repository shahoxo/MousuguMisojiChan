using System.Collections;
using System.Collections.Generic;

public class MisojiChanGame {
	public int age = 0;
	public Player firstPlayer;
	public Player secondPlayer;
	public Player winner;

	public MisojiChanGame() {
      	firstPlayer = new Player() { name = "1P" };
	  	secondPlayer = new Player() { name = "2P" };
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
		if(IsEnding())
			winner = FindWinner(player);
	}

	public bool IsEnding() {
		return age >= 30;
	}

	Player FindWinner(Player player) {
        return firstPlayer == player ? secondPlayer : firstPlayer;
	}

	public class Player  {
		public string name;
	}
}
