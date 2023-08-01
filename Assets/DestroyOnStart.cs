using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnStart : NetworkBehaviour
{  
    

    public override void Spawned()
    {
        Spawner.OnGameStart += () => Runner.Despawn(Object);
    }
}
