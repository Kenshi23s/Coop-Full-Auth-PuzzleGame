using UnityEngine;
using Fusion;
[RequireComponent(typeof(NetworkTransform))]
public class FreezeRotation : NetworkBehaviour
{
    NetworkTransform ntwk_transform;
    private void Awake()
    {
        ntwk_transform = GetComponent<NetworkTransform>();
    }

    public override void FixedUpdateNetwork()
    {
        ntwk_transform.TeleportToRotation(Quaternion.Euler(0, 0, 0));
    }   
}
