using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Imphenzia;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SocialPlatforms.Impl;

public class GameController : SingletoneGameObject<GameController>
{
    [SerializeField] public TilesController TilesController = null;
    [SerializeField] public MovementBall BallController = null;
    [SerializeField] public ScoreManager ScoreManager = null;
    [SerializeField] private GradientSkyCamera _gradientSkyCamera;

    public event Action EventGo;

    protected override void Awake()
    {
        base.Awake();
        BallController.EventStop += OnPauseGame;
        BallController.EventPassed += OnPassedSession;
    }


    private void Start()
    {
        ((UIStartPanel)UIManager.Instance.GetPanel(UITypePanel.Start)).EventStart += OnStart;

        ((UIInfoPanel)UIManager.Instance.GetPanel(UITypePanel.Info)).EventGoGame += OnGoGame;

        ((UIRestartPanel)UIManager.Instance.GetPanel(UITypePanel.Restart)).EventStart += OnReStart;
        ((UIRestartPanel)UIManager.Instance.GetPanel(UITypePanel.Restart)).EventContinue += OnContinue;

        var gradient = GradientManager.Instance.getGradient();
        _gradientSkyCamera.SetGradient(gradient.colorKeys, gradient.alphaKeys);

        TilesController.ResetLevel();
    }

    private void OnDestroy()
    {
        if (TryInstance != null)
        {
            BallController.EventStop -= OnPauseGame;
            BallController.EventPassed -= OnPassedSession;

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
        UIManager.Instance.ShowPanel(UITypePanel.GamePlay);
        EventGo?.Invoke();
    }

    private void OnReStart()
    {
        UIManager.Instance.ShowPanel(UITypePanel.Start);
        var platforms = TilesController.GetComponentsInChildren<MovementTile>().ToList();
        foreach (var platform in platforms)
        {
            Destroy(platform.gameObject);
        }
        TilesController.ResetLevel();
    }

    private void OnPauseGame()
    {
        UIManager.Instance.ShowPanel(UITypePanel.Restart);
        BallController.DefaultPosition();
    }

    private void OnContinue()
    {
        UIManager.Instance.ShowPanel(UITypePanel.Info);
    }

    private void OnPassedSession()
    {
        UIManager.Instance.ShowPanel(UITypePanel.Restart);
        BallController.DefaultPosition();
        TilesController.NextSession();
        ScoreManager.ResetSessionScore();
        TilesController.NextLevel();
        GradientManager.Instance.GenerateGradient();
        var gradient = GradientManager.Instance.getGradient();
        _gradientSkyCamera.SetGradient(gradient.colorKeys, gradient.alphaKeys);
    }
}
