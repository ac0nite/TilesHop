using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private int _scoreValue = 0;
    private int _scoreValueSession = 0;
    [SerializeField] private Text _scoreTextUI;
    private int _recordValue = 0;
    [SerializeField] private Text _recordTextUI;
    [SerializeField] private MovementBall _movementBall = null;
    [SerializeField] private TilesController _tiles = null;

    private void Awake()
    {
        _recordValue = PlayerPrefs.GetInt("Record", 0);
        _recordTextUI.text = _recordValue.ToString();
        _scoreTextUI.text = _scoreValue.ToString();
        _movementBall.EventAddScore += OnChangeScore;
        _tiles.EventFinal += OnChangeScoreRecord;
        _tiles.EventFirst += OnGo;
    }

    private void OnGo()
    {
        _scoreValueSession = 0;
    }

    public void ResetAllScore()
    {
        _scoreValue = 0;
        _scoreValueSession = 0;
        _scoreTextUI.text = _scoreValue.ToString();
    }

    public void ResetSessionScore()
    {
        _scoreValueSession = 0;
    }

    public int Score()
    {
        return _scoreValue;
    }
    
    public int ScoreSession()
    {
        return _scoreValueSession;
    }

    private void OnDestroy()
    {
        _tiles.EventFinal -= OnChangeScoreRecord;
        _movementBall.EventAddScore -= OnChangeScore;
        _tiles.EventFirst -= OnGo;
    }

    private void OnChangeScore()
    {
        _scoreValue++;
        _scoreValueSession++;
        _scoreTextUI.text = _scoreValue.ToString();
    }

    private void OnChangeScoreRecord()
    {
        _recordValue = Mathf.Clamp(_scoreValue, _recordValue, Int32.MaxValue);
        _recordTextUI.text = _recordValue.ToString();
        PlayerPrefs.SetInt("Record", _recordValue);
        PlayerPrefs.Save();
    }
}
