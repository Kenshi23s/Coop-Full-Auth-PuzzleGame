using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;

public class Spawner : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] NetworkPlayer _prefabVariant1;
    [SerializeField] NetworkPlayer _prefabVariant2;
    CharacterInputHandler _characterInputHandler;

    int count = 0;

    public static event Action GameStart = delegate{ };
    //Callback que se recibe cuando entra un nuevo Cliente a la sala
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            
            var z = runner.Spawn(GetVariant(count), new Vector3(1,2,0), null, player);
            z.SetElement(count == 0 ? Element.Fire : Element.Water);
            count++;
            if (count > 1)
            {
                count = 0;
                GameStart();
                GameStart = delegate { };
            }
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
