﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GradientManager : SingletoneGameObject<GradientManager>
{
    private float _evalute = 0f;
    private Gradient _gradient = null;
    private GradientColorKey[] colorKeys;
    private GradientAlphaKey[] alphaKeys;
    private Color _firstColor;
    private Color _secondColor;
    protected override void Awake()
    {
        base.Awake();
        _firstColor = RandomColor();
        _secondColor = RandomColor();
        _gradient = new Gradient();
        colorKeys = new GradientColorKey[6];
        alphaKeys = new GradientAlphaKey[6];
        GenerateGradient();
    }

    void Start()
    {
    }

    private Color RandomColor()
    {
        return new Color(Gen(), Gen(), Gen());
    }

    public void GenerateGradient()
    {
        Debug.Log($"first: {_firstColor}  second: {_secondColor}");
        _evalute = 0f;
        
        // colorKeys[0].color = Color.green;
        // colorKeys[0].time = 1.0F;
        // colorKeys[1].color = Color.red;
        // colorKeys[1].time = -1.0F;
        // colorKeys[2].color = Color.yellow;
        // colorKeys[2].time = -1.0F;
        //
        // alphaKeys[0].alpha = 0.0F;
        // alphaKeys[0].time = 1.0F;
        // alphaKeys[1].alpha = 0.0F;
        // alphaKeys[1].time = -1.0F;
        // alphaKeys[1].alpha = 0.0F;
        // alphaKeys[1].time = -1.0F;
        float steps = 6 - 1f;
        for (int i = 0; i < 6; i++)
        {
            float step = i / steps;
            colorKeys[i].color = RandomColor();
            colorKeys[i].time = step;
            alphaKeys[i].alpha = 0.0F;
            alphaKeys[i].time = step;
        }
        
        _gradient.SetKeys(colorKeys, alphaKeys);
    }

    private float Gen()
    {
        return UnityEngine.Random.Range(0f, 1f);
    }

    public Color GetColor()
    {
        _evalute += 0.02f;
        if (_evalute >= 1)
        {
            _evalute = 0f;
            _firstColor = _secondColor;
            _secondColor = RandomColor();
            GenerateGradient();
        }
        return _gradient.Evaluate(_evalute);
    }

    public Color GetFromColor()
    {
        return _gradient.Evaluate(0f);
    }

    public Color GetToColor()
    {
        return _gradient.Evaluate(1f);
    }
}
