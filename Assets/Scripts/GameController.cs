using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SocialPlatforms.Impl;

public class GameController : SingletoneGameObject<GameController>
{
    [SerializeField] public TilesController TilesController = null;
    [SerializeField] public MovementBall BollController = null;
    [SerializeField] public ScoreManager ScoreManager = null;
   // [SerializeField] private UIInfoPanel _infoPanel = null;

    public event Action EventStart;
    public event Action EventPause;
    public event Action EventGo;
    public event Action EventStop;

    protected override void Awake()
    {
        base.Awake();

        //_infoPanel.EventGoGame += OnGoGame;
        BollController.EventStop += OnPauseGame;
        BollController.EventPassed += OnPassedSession;
    }

    // private void Awake()
    // {
    //     _infoPanel.EventGoGame += OnGoGame;
    //     //((UIRestartPanel) UIManager.Instance.GetPanel(UITypePanel.Start)).EventStart += OnStart;
    // }

    private void Start()
    {
        ((UIStartPanel)UIManager.Instance.GetPanel(UITypePanel.Start)).EventStart += OnStart;

        ((UIInfoPanel)UIManager.Instance.GetPanel(UITypePanel.Info)).EventGoGame += OnGoGame;

        ((UIRestartPanel)UIManager.Instance.GetPanel(UITypePanel.Restart)).EventStart += OnReStart;
        ((UIRestartPanel)UIManager.Instance.GetPanel(UITypePanel.Restart)).EventContinue += OnContinue;
    }

    private void OnDestroy()
    {
        //((UIRestartPanel) UIManager.Instance.GetPanel(UITypePanel.Start)).EventStart -= OnStart;
        if (TryInstance != null)
        {
           // _infoPanel.EventGoGame -= OnGoGame;
            BollController.EventStop -= OnPauseGame;
            BollController.EventPassed -= OnPassedSession;

            ((UIStartPanel)UIManager.Instance.GetPanel(UITypePanel.Start)).EventStart -= OnStart;

            ((UIInfoPanel)UIManager.Instance.GetPanel(UITypePanel.Info)).EventGoGame -= OnGoGame;

            ((UIRestartPanel)UIManager.Instance.GetPanel(UITypePanel.Restart)).EventStart -= OnReStart;
            ((UIRestartPanel)UIManager.Instance.GetPanel(UITypePanel.Restart)).EventContinue -= OnContinue;
        }
    }

    private void OnStart()
    {
        Debug.Log($"OnStart()");
        UIManager.Instance.ShowPanel(UITypePanel.Info);
        ScoreManager.ResetAllScore();
        GradientManager.Instance.GenerateGradient();
    }

    private void OnGoGame()
    {
        Debug.Log($"OnGoGame()");
        UIManager.Instance.ShowPanel(UITypePanel.GamePlay);
        EventGo?.Invoke();
    }

    private void OnReStart()
    {
        Debug.Log($"OnReStart()");
        UIManager.Instance.ShowPanel(UITypePanel.Start);
        var platforms = TilesController.GetComponentsInChildren<MovementTile>().ToList();
        foreach (var platform in platforms)
        {
            Destroy(platform.gameObject);
        }
    }

    private void OnPauseGame()
    {
        Debug.Log($"OnPauseGame()");
        UIManager.Instance.ShowPanel(UITypePanel.Restart);
        BollController.DefaultPosition();
    }

    private void OnContinue()
    {
        Debug.Log($"OnContinue()");
        UIManager.Instance.ShowPanel(UITypePanel.Info);
        //EventGo?.Invoke();
    }

    private void OnPassedSession()
    {
        UIManager.Instance.ShowPanel(UITypePanel.Restart);
        BollController.DefaultPosition();
        TilesController.NextSession();
        ScoreManager.ResetSessionScore();
    }
}
