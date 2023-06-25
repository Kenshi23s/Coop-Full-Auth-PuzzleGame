using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnStart : NetworkBehaviour
{
  
    private void Awake() => Spawner.GameStart += () => Runner.Despawn(Object);
}
