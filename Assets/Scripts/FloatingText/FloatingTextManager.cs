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
    FloatingText _sampleFloatingText;

 
  
    void Awake()
    {
        instance = this;
        _debug = GetComponent<DebugableObject>();   
       
    }

    //esto deberia subscribirse a el gun manager.actualgunhit<HitData>, por ahora lo dejo asi
    public void PopUpText(string text,Vector3 pos)
    {

        FloatingText t = Instantiate(_sampleFloatingText,pos,Quaternion.identity);
        if (t == null) return;
                           
        _parameters.IncreaseSortingOrder();           
        t.InitializeText(text, pos, _parameters, new Color(143, 0, 255)); // violeta
        
    }
}
