using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class LobbyHandler : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] NetworkRunner _runnerPrefab;
    NetworkRunner _currentRunner;

    public event Action OnJoinedLobby;
    public event Action OnFailedJoinLobby;

    public bool testing;
     
    public event Action<List<SessionInfo>> OnSessionListUpdate;


    private void Start()
    {
        if (testing)
        {
            JoinLobby();
            OnJoinedLobby += () => CreateSession(Random.Range(0, 1000).ToString(), "Level");
        }
       
    }
    #region LOBBY

    //Lo agregamos a un boton luego
    public void JoinLobby()
    {
        if (_currentRunner) Destroy(_currentRunner.gameObject);

        _currentRunner = Instantiate(_runnerPrefab);


        _currentRunner.AddCallbacks(this);


        var clientTask = JoinLobbyTask();
    }

    async Task JoinLobbyTask()
    {
        var result = await _currentRunner.JoinSessionLobby(SessionLobby.Custom, "Normal Lobby");

        if (!result.Ok)
        {
            Debug.LogError("[Custom Error] Unable to Join Lobby");
            //evento que vuelva al panel q aparece para unirse al lobby
            OnFailedJoinLobby?.Invoke();
        }
        else
        {
            Debug.Log("[Custom Msg] Lobby Joined");

            OnJoinedLobby?.Invoke();
        }
    }

    #endregion
   

    #region CREATE/JOIN SESSION

    //Lo agregamos a un boton luego
    public void CreateSession(string sessionName, string sceneName)
    {
        var clientTask = InitializeSession(_currentRunner, GameMode.Host, sessionName, SceneUtility.GetBuildIndexByScenePath($"Scenes/{sceneName}"));
    }

    //Lo agregamos a un boton luego
    public void JoinSession(SessionInfo sessionInfo)
    {
        var clientTask = InitializeSession(_currentRunner, GameMode.Client, sessionInfo.Name, SceneManager.GetActiveScene().buildIndex); 
    }

    async Task InitializeSession(NetworkRunner runner, GameMode gameMode, string sessionName, SceneRef scene)
    {
        var sceneManager = runner.GetComponent<NetworkSceneManagerDefault>();

        var result = await runner.StartGame(new StartGameArgs
        {
            GameMode = gameMode,
            Scene = scene,
            SessionName = sessionName,
            CustomLobbyName = "Normal Lobby",
            SceneManager = sceneManager
        });

        if (!result.Ok)
        {
            Debug.LogError("[Custom Error] Unable to Start Game");
        }
        else
        {
            Debug.Log("[Custom Msg] Game Started");
        }
    }

    #endregion

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        OnSessionListUpdate?.Invoke(sessionList);       
    }

    #region Unused Callbacks
    public void OnConnectedToServer(NetworkRunner runner) { }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }

    public void OnDisconnectedFromServer(NetworkRunner runner) { }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }

    public void OnInput(NetworkRunner runner, NetworkInput input) { }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) { }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }

    public void OnSceneLoadDone(NetworkRunner runner) { }

    public void OnSceneLoadStart(NetworkRunner runner) { }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }

    #endregion
}
