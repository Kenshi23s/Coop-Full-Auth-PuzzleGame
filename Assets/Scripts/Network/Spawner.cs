using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;
using System.Linq;

public class Spawner : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] NetworkPlayer _prefabVariant1;
    [SerializeField] NetworkPlayer _prefabVariant2;
    CharacterInputHandler _characterInputHandler;

  

     static int _charactersCount = 0;
    static int _playersReady = 0;

     public static event Action OnGameStart = delegate{ };
     public static NetworkRunner NTWKrunner { get; private set; }

    static List<NetworkPlayer> connectedPlayers = new List<NetworkPlayer>();


   
    public static void Teleport()
    {
        Debug.Log("Teletransporto a " + connectedPlayers.Count +"jugadores al inicio");
        Vector3 _startPos = GameManager.instance.LevelStartPös.position;
        foreach (var item in connectedPlayers)
        {
            Debug.Log("teletransporto a " + item);
            item.ntwk_transform.TeleportToPosition(_startPos);
        }
      
      
    }
    //Callback que se recibe cuando entra un nuevo Cliente a la sala
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        NTWKrunner = runner;
        if (runner.IsServer)
        {
            Vector3 lobbyPos = GameManager.instance.LobbyPos.position;
            var newPj = runner.Spawn(GetVariant(_charactersCount), lobbyPos, null, player);
            newPj.ntwk_transform.TeleportToPosition(lobbyPos); 
            connectedPlayers.Add(newPj);
            newPj.SetElement(_charactersCount == 0 ? Element.Fire : Element.Water);
            _charactersCount++;

        }
        else
        {
            Debug.Log("Player Joined, I'm not the server/host");
        }
    }



    NetworkPlayer GetVariant(int x)
    {
        return x == 0 ? _prefabVariant1 : _prefabVariant2;
    }

    public static void PlayerReady()
    {
        _playersReady++;
        Debug.Log($"Un jugador mas esta listo, hay {_playersReady} preparados");
        if (_playersReady > 1)
        {
            _playersReady = 0;
            Teleport();
            Debug.Log("Todos listos, Empieza la partida!");
            OnGameStart();
           
            OnGameStart = delegate { };
        }
    }

    public static void PlayerNotReady()
    {
        _playersReady = (int)MathF.Max(_playersReady - 1,0);
        Debug.Log($"Un jugador menos esta listo, hay {_playersReady} preparados");
    }





    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        if (!NetworkPlayer.Local) return;

        if (!_characterInputHandler)
        {
            _characterInputHandler = NetworkPlayer.Local.GetInputHandler();
        }
        else
        {
            input.Set(_characterInputHandler.GetInputs());
        }
    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {     
        runner.Shutdown();
    }

    #region Unused Callbacks

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList){ }

    public void OnConnectedToServer(NetworkRunner runner) { }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }

 

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }

    public void OnSceneLoadDone(NetworkRunner runner) { }

    public void OnSceneLoadStart(NetworkRunner runner) { }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) {  }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }

    #endregion
}
