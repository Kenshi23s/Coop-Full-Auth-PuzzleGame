using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;

public class Spawner : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] NetworkPlayer _playerPrefab;
    CharacterInputHandler _characterInputHandler;


    //Callback que se recibe cuando entra un nuevo Cliente a la sala
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            
            runner.Spawn(_playerPrefab, Vector3.zero, null, player);
        }
        else
        {
            Debug.Log("Player Joined, I'm not the server/host");
        }
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

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }

    #endregion
}
