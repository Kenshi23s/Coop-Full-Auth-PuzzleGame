using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Element
{
    Fire,Water
}
[System.Serializable]
public struct ElementData
{
    public LayerMask elementLayer;
    public Material ElementMaterial;
    public GameObject SplashParticle;
    public GameObject enviromentParticle;
}
public class LayerManager : MonoBehaviour
{    
   

    [SerializeField]
    ElementData FireElementData, WaterElementData;
    public ElementData GetElementData(Element x)
    {
        ElementData newData= new ElementData();
        if (x==Element.Fire)
        {
            newData = FireElementData;
        }
        else
        {
            newData = WaterElementData;
        }

        return newData;
    }
      

    public static LayerManager instance;
    private void Awake()
    {
        instance = this;
    }
}
