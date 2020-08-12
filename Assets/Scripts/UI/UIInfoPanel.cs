using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInfoPanel : UIBasePanel
{
    public event Action EventGoGame;

    private void Awake()
    {
        if(Input.GetMouseButtonDown(0))
            EventGoGame?.Invoke();
    }

    private void Update()
    {
        if(Input.GetMouseButton(0))
            EventGoGame?.Invoke();
    }
}
