using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRestartPanel : UIBasePanel
{
    [SerializeField] private Text _percentComplited;
    public event Action EventContinue;
    public event Action EventStart;
    private void Awake()
    {
        _percentComplited.text = GameController.Instance.TilesController.PercentSession().ToString();
    }

    private void Update()
    {
        if(_percentComplited.text != GameController.Instance.TilesController.PercentSession().ToString())
            _percentComplited.text = GameController.Instance.TilesController.PercentSession().ToString();
    }

    public void PressButtonMenu()
    {
        EventStart?.Invoke();
        //UIManager.Instance.ShowPanel(UITypePanel.Start);
    }

    public void PressButtonContinue()
    {
            //UIManager.Instance.ShowPanel(UITypePanel.Info);
            EventContinue?.Invoke();
    }
}
