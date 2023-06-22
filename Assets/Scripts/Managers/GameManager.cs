using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool runtime = false;
 

    public static GameManager instance;

   public float zOffset;
    

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
      

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
}
