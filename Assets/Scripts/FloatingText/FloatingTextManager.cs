using UnityEngine;
using static FloatingText;
[RequireComponent(typeof(DebugableObject))]
public class FloatingTextManager : MonoBehaviour
{

    DebugableObject _debug;

    public static FloatingTextManager instance;

    [SerializeField]
    FloatingTextParam _parameters;
    [SerializeField]
    FloatingText sampleFloatingText;

 
  
    void Awake()
    {
        instance = this;
        _debug = GetComponent<DebugableObject>();   
       
    }

    //esto deberia subscribirse a el gun manager.actualgunhit<HitData>, por ahora lo dejo asi
    public void PopUpText(string text,Vector3 pos, bool isCrit = false)
    {

        FloatingText t = Instantiate(sampleFloatingText,pos,Quaternion.identity);

        if (t != null)
        {                         
            _parameters.IncreaseSortingOrder();           
            t.InitializeText(text, pos, _parameters, Color.cyan);
           

        }
    }
}
