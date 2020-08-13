using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class TilesController : MonoBehaviour
{
    [SerializeField] private MovementTile prefabBlock;

    [SerializeField] private float _startSpeed = 5f;
    [SerializeField] private int _startCountPlatform = 10; 
    [SerializeField] private int _percentInfo = 20;
    [SerializeField] private Text _infoText;

    private int _countComlited = 0;
    private MovementTile _backPlatform = null;
    private float _nextSizeCollider = 0f;
    public event Action EventFirst;
    public event Action<bool> EventMovement;
    public event Action EventFinal;
    private bool _go = false;

    private int _currentPlatform = 0;
    private float _currentSpeed = 0;

    private void Awake()
    {
        GameController.Instance.EventGo += OnStart;
        GameController.Instance.BallController.EventStop += OnStop;
        _infoText.text = "";
        _currentPlatform = _startCountPlatform;
        _currentSpeed = _startSpeed;
    }

    public void NextLevel()
    {
        _currentPlatform = Random.Range(_currentPlatform + 1, _currentPlatform + 10);
        _currentSpeed += 0.5f;
    }

    public void ResetLevel()
    {
        _currentPlatform = _startCountPlatform;
        _currentSpeed = _startSpeed;
    }

    private void OnDestroy()
    {
        GameController.Instance.EventGo -= OnStart;
        GameController.Instance.BallController.EventStop -= OnStop;
    }

    public void NextSession()
    {
        _go = false;
        _countComlited = 0;
    }

    private void OnStart()
    {
        _go = true;
        if (GetComponentsInChildren<MovementTile>().ToList().Count == 0)
        {
            RandomSizeNextCollider();
            CreatePlatform(_nextSizeCollider);
            EventFirst?.Invoke();
        }
        else
        {
            EventMovement?.Invoke(true);
            if(PercentSession() > 0f)
                _countComlited--;
        }
    }

    private void OnStop()
    {
        _go = false;
        EventMovement?.Invoke(false);
    }
    
    void Start()
    {
    }

    public float PercentSession()
    {
        return (float) GameController.Instance.ScoreManager.ScoreSession() / (float)_currentPlatform * 100f;
    }

    public bool EndSession()
    {
     //   Debug.Log($"{PercentSession()} {_countComlited}");
        return PercentSession() >= 100f;
    }

    void Update()
    {
        if (!_go) return;
        if (_countComlited >= _currentPlatform)
        {
            if (_backPlatform.Completion)
            {
                EventFinal?.Invoke();
                _go = false;
            }
            return;   
        }

        var s1 = _backPlatform.GetSizeCollider().z / 2f + _nextSizeCollider / 2f;
        var s2 = (transform.position - _backPlatform.transform.position).magnitude;
        
        if ((transform.position - _backPlatform.transform.position).magnitude >= (_backPlatform.GetSizeCollider().z / 2f  + _nextSizeCollider / 2f))
        {
            CreatePlatform(_nextSizeCollider);
        }
    }

    void CreatePlatform(float lenghtCollider)
    {
        _backPlatform = Instantiate(prefabBlock, transform);
        _backPlatform.transform.position = RandomStartPosition();
        _backPlatform.SetSizeCollider(lenghtCollider);
        _backPlatform.Speed = _currentSpeed;
        _backPlatform.transform.SetParent(transform);
        RandomSizeNextCollider();
        _countComlited++;

        var percent = PercentSession();

        if (percent != 0 && percent % _percentInfo == 0)
            StartCoroutine(SetInfo($"{percent}% complited"));

    }

    void RandomSizeNextCollider()
    {
        _nextSizeCollider = Random.Range(prefabBlock.GetSizePlatform().z, prefabBlock.GetSizePlatform().z + 4f);
    }

    Vector3 RandomStartPosition()
    {
        return new Vector3(transform.position.x + Random.Range(-1.2f, 1.2f), transform.position.y,transform.position.z );
    }

    IEnumerator SetInfo(String txt)
    {
        _infoText.text = txt;
        yield return new WaitForSeconds(2f);
        _infoText.text = "";
    }
}
