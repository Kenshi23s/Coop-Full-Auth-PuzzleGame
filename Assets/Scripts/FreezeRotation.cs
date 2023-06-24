using UnityEngine;
using Fusion;
using System.Diagnostics;

[RequireComponent(typeof(NetworkTransform))]
[RequireComponent(typeof(DebugableObject))]
public class FreezeRotation : NetworkBehaviour
{
    NetworkTransform ntwk_transform;
    DebugableObject _debug;
   [SerializeField] Obstacle myObstacle;
    private void Awake()
    {
        ntwk_transform = GetComponent<NetworkTransform>();
        _debug = GetComponent<DebugableObject>();
        myObstacle.PlayerEnter += PlayerEnter;
        myObstacle.PlayerLeave += PlayerLeave;
    }

    public override void FixedUpdateNetwork()
    {
        //para que la plataforma siempre mire hacia arriba
        ntwk_transform.TeleportToRotation(Quaternion.Euler(0, 0, 0));
    }

    
    public void PlayerEnter(NetworkPlayer player)
    {
        _debug.Log($"Hago que {player} mi hijo");
        if (player.transform.parent!=transform)
        {
            player.transform.parent = transform;
        }
    }

    public void PlayerLeave(NetworkPlayer player)
    {
        _debug.Log($"Hago que {player} ya NO sea mi hijo");
        if (player.transform.parent == transform)
        {           
            player.transform.parent = null;
        }
    }
   
   
}
