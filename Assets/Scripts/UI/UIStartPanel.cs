using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStartPanel : UIBasePanel
{
    [SerializeField] private Text _record = null;
    [SerializeField] private Text _lostScore = null;
    public event Action EventStart;

    private void Awake()
    {
        _record.text = PlayerPrefs.GetInt("Record", 0).ToString();
        _lostScore.text = GameController.Instance.ScoreManager.Score().ToString();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
            EventStart?.Invoke();

        if(_lostScore.text != GameController.Instance.ScoreManager.Score().ToString())
            _lostScore.text = GameController.Instance.ScoreManager.Score().ToString();
    }
}
