using System.Diagnostics;
using UnityEngine;
[RequireComponent(typeof(DebugableObject))]
public class Teleporter : MonoBehaviour ,Iinteractable
{

    [SerializeField]
    Teleporter TPTo;
    DebugableObject debug;

    public void Interact(NetworkPlayer whoInteracted)
    {
        if (TPTo == null) 
        {
            return;
        }
        debug.Log("Teletransporto a" + whoInteracted.name);
        whoInteracted.ntwk_transform.TeleportToPosition(new Vector3(TPTo.transform.position.x,
                                                       TPTo.transform.position.y,
                                                       whoInteracted.transform.position.z));
        //whoInteracted.transform.position = new Vector3(TPTo.transform.position.x, 
        //                                               TPTo.transform.position.y,
        //                                               whoInteracted.transform.position.z);
    }

    private void Awake()
    {
        debug = GetComponent<DebugableObject>();
        debug.AddGizmoAction(DrawLineToTp);
    }


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
    void Interact(NetworkPlayer whoInteracted);
}
