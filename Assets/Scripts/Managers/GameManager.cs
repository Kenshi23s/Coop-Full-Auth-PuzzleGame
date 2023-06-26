using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour,INetworkRunnerCallbacks
{
    public static bool runtime = false;

    [SerializeField]public const string WinScene = "Victory";
    [SerializeField]public const string LoseScene = "Loss";

    Dictionary<Element, bool> winObject = new Dictionary<Element, bool>();


    public static GameManager instance;
    public event Action OnGameEnd;

    public float zOffset;
    

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
        winObject.Add(Element.Fire, false);
        winObject.Add(Element.Water, false);


        runtime = true;
    }
    public override void Spawned()
    {
        base.Spawned();
        Runner.AddCallbacks(this);
    }
    public void SetCamera(NetworkPlayer player)
    {
        //lo hardcodeo pq al hacerlo network object ya no pude editar las variables desde editor C:
        Camera.main.transform.position = player.transform.position + Vector3.back * 100;
        Camera.main.transform.forward = (player.transform.position - Camera.main.transform.position).normalized;
       
    }  

    [Rpc(RpcSources.StateAuthority,RpcTargets.All)]
    public void RPC_GAMEOVER(bool has_Won)
    {
        
        Debug.Log(has_Won ? "Victoria" : "Derrota");
        OnGameEnd?.Invoke();
    
        if (HasStateAuthority)
        {
            string scene = has_Won ? WinScene : LoseScene;
            RPC_SENDTOMENU(scene);
            SceneManager.LoadScene(scene);
         
            Runner.Shutdown();
           
        }
       


    }
    [Rpc(RpcSources.All, RpcTargets.Proxies)]
    void RPC_SENDTOMENU(string scene)
    {
        if (HasStateAuthority) return;

        Debug.Log("Soy Proxie, vuelvo a la escena");
        SceneManager.LoadScene(scene);
            Runner.Shutdown();
         
    }

    [Rpc(RpcSources.All,RpcTargets.StateAuthority)]
    public void RPC_SetElement(Element x,bool arg)
    {
        winObject[x] = arg;

        foreach (var item in winObject) if (!item.Value) { Debug.Log("Condiciones NO aprobadas"); return; } 

        Debug.Log("Condiciones aprobadas, pasando a game over de victoria");
        RPC_GAMEOVER(true);
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        throw new NotImplementedException();
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
       runner.Shutdown();
       SceneManager.LoadScene(LoseScene);
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        throw new NotImplementedException();
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        throw new NotImplementedException();
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        throw new NotImplementedException();
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        throw new NotImplementedException();
    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
        throw new NotImplementedException();
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        throw new NotImplementedException();
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        throw new NotImplementedException();
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        throw new NotImplementedException();
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        throw new NotImplementedException();
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        throw new NotImplementedException();
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        throw new NotImplementedException();
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
        throw new NotImplementedException();
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        throw new NotImplementedException();
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        throw new NotImplementedException();
    }
}
