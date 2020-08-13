using System.Collections;
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

    public Gradient getGradient()
    {
        return _gradient;
    }

    private Color RandomColor()
    {
        return new Color(Gen(), Gen(), Gen());
    }

    public void GenerateGradient()
    {
        //Debug.Log($"first: {_firstColor}  second: {_secondColor}");
        _evalute = 0f;
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
        _evalute += 0.03f;
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
