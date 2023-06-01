using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Element
{
    Fire,Water
}

public class LayerManager : MonoBehaviour
{    
    [SerializeField] LayerMask FireLayer, WaterLayer;
    [SerializeField] Material FireMaterial, WaterMaterial;
   
    public Tuple<LayerMask,Material> GetLayer(Element i) => i == Element.Fire 
        ? Tuple.Create(FireLayer,FireMaterial) 
        : Tuple.Create(WaterLayer, WaterMaterial);

    public static LayerManager instance;
    private void Awake()
    {
        instance = this;
    }
}
