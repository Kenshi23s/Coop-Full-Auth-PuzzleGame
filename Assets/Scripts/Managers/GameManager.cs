using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Progress;

public class GameManager : NetworkObject
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
            Runner.Shutdown();
            SceneManager.LoadScene(scene);
        }
       


    }
    [Rpc(RpcSources.StateAuthority, RpcTargets.Proxies)]
    void RPC_SENDTOMENU(string scene)
    {
        Runner.SetActiveScene(scene);
        Runner.Shutdown();
         
    }

    [Rpc(RpcSources.All,RpcTargets.StateAuthority)]
    public void SetElement(Element x,bool arg)
    {
        winObject[x] = arg;

        foreach (var item in winObject) if (!item.Value) { Debug.Log("Condiciones NO aprobadas"); return; } 

        Debug.Log("Condiciones aprobadas, pasando a game over de victoria");
        RPC_GAMEOVER(true);
    }
    
    
}
