using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorTile : MonoBehaviour
{
    void Start()
    {
        var renderer = GetComponent<Renderer>();
        Material material = renderer.material;
        material.SetColor("_Color", GradientManager.Instance.GetColor());
        renderer.material = material;
 
    }
}
