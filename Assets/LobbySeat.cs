using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbySeat : NetworkBehaviour
{
    [SerializeField]
    Element myElement;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (!HasStateAuthority) return;

        if (other.TryGetComponent(out NetworkPlayer player))
        {
            if (player.gameObject.layer != gameObject.layer) return;

            Debug.Log("Mi layer coincide con la del player, lo alisto para la partida");
            Spawner.PlayerReady();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!HasStateAuthority) return;

        if (other.TryGetComponent(out NetworkPlayer player))
        {
            if (player.gameObject.layer != gameObject.layer) return;
            Debug.Log("Mi layer coincide con la del player, le saco el listo");
            Spawner.PlayerNotReady();
        }
    }

}
