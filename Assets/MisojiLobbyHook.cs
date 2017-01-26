using UnityEngine;
using UnityEngine.Networking;

public class MisojiLobbyHook : Prototype.NetworkLobby.LobbyHook {

	public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer) {
		if(lobbyPlayer == null)	
			return;
		Prototype.NetworkLobby.LobbyPlayer _lobbyPlayer = lobbyPlayer.GetComponent<Prototype.NetworkLobby.LobbyPlayer>();
		if(_lobbyPlayer != null)
			GameManager.AddPlayer(gamePlayer, _lobbyPlayer.nameInput.text, _lobbyPlayer.playerControllerId);
	}
}
