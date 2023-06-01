using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(DebugableObject))]
public class Teleporter : MonoBehaviour
{

    [SerializeField]
    Teleporter TPTo;

    private void Awake()
    {
        GetComponent<DebugableObject>().AddGizmoAction(DrawLineToTp);
    }


    void DrawLineToTp()
    {
        if (TPTo == null) return;
        Color color = Color.green;
        Vector3 dir = TPTo.transform.position - transform.position;
        DrawArrow.ForGizmo(transform.position, dir.normalized * 5,color,1,30);
    }

}
