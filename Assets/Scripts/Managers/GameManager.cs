using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        Camera.main.transform.position = player.transform.position + Vector3.back * zOffset;
        Camera.main.transform.forward = (player.transform.position - Camera.main.transform.position).normalized;
       
    }  
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [Rpc(RpcSources.All,RpcTargets.All)]
    public void RPC_GAMEOVER(bool has_Won)
    {
        Debug.Log(has_Won ? "Victoria" : "Derrota");
        OnGameEnd?.Invoke();
        SceneManager.LoadScene(has_Won ? WinScene : LoseScene);
    }

    [Rpc(RpcSources.All,RpcTargets.StateAuthority)]
    public void SetElement(Element x,bool arg)
    {
        winObject[x] = arg;

        foreach (var item in winObject) if (!item.Value) { Debug.Log("condiciones NO aprobadas"); return; } 


        Debug.Log("condiciones aprobadas, pasando a game over de victoria");
        RPC_GAMEOVER(true);
    }
    
    
}
