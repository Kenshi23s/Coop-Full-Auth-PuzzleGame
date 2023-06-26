using Fusion;
using System.Diagnostics;
using UnityEngine;
[RequireComponent(typeof(DebugableObject))]
public class Teleporter : MonoBehaviour ,Iinteractable
{

    [SerializeField]
    Teleporter TPTo;
    DebugableObject _debug;
    LineRenderer render;

    [Rpc(RpcSources.All,RpcTargets.StateAuthority)]
    public void RPC_Interact(NetworkPlayer whoInteracted)
    {
        if (TPTo == null) return;
        _debug.Log("Teletransporto a" + whoInteracted.name);
        whoInteracted.ntwk_transform.TeleportToPosition(new Vector3(TPTo.transform.position.x,
                                                       TPTo.transform.position.y,
                                                       whoInteracted.transform.position.z));
    }

    private void Awake()
    {
        _debug = GetComponent<DebugableObject>();
        _debug.AddGizmoAction(DrawLineToTp);
        render = GetComponent<LineRenderer>();

        if (TPTo == null) return;
        
        Vector3 offsetZ = new Vector3(0, 0, -5);
        render.SetPosition(0,transform.position - offsetZ);
        render.SetPosition(1, TPTo.transform.position - offsetZ);
    }

    //debug Gizmo
    void DrawLineToTp()
    {
        if (TPTo == null) return;
        Color color = Color.green;
        Vector3 dir = TPTo.transform.position - transform.position;
        DrawArrow.ForGizmo(transform.position, dir.normalized * 5,color,1,30);
        Gizmos.DrawWireSphere(TPTo.transform.position, 2f);
    }
}
public interface Iinteractable
{
    void RPC_Interact(NetworkPlayer whoInteracted);
}
