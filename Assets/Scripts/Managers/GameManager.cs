using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool runtime = false;
    [SerializeField]public NetworkPlayer _playerPrefab;
    [SerializeField] public static NetworkPlayer playerPrefab;
    public static GameManager instance;

    float zOffset;
    

    // Start is called before the first frame update
    private void Awake()
    {
   
        playerPrefab = _playerPrefab;
        runtime = true;
    }

    public void SetCamera(NetworkPlayer player)
    {
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, player.transform.position + Vector3.back * zOffset,Time.deltaTime*3f);
        Camera.main.transform.forward = player.transform.forward - Camera.main.transform.position;
    }  
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
