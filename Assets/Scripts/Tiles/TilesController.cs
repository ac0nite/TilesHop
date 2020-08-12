using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class TilesController : MonoBehaviour
{
    [SerializeField] private MovementTile prefabBlock;
    //[SerializeField] private float _distance = 5f;

    [SerializeField] private float _speed = 5f;
    [SerializeField] private int _countPlatform = 10; 
    [SerializeField] private int _percentInfo = 20;
    private int _countComlited = 0;
    private MovementTile _backPlatform = null;
    private float _nextSizeCollider = 0f;
    
    public event Action EventFirst;
    public event Action<bool> EventMovement;
    public event Action EventFinal;

    private bool _go = false;
    [SerializeField] private Text _infoText;

    private void Awake()
    {
        GameController.Instance.EventGo += OnStart;
        GameController.Instance.BollController.EventStop += OnStop;
    }

    private void OnDestroy()
    {
        GameController.Instance.EventGo -= OnStart;
        GameController.Instance.BollController.EventStop -= OnStop;
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
        // var platforms = GetComponentsInChildren<MovementTile>().ToList();
        // foreach (var platform in platforms)
        // {
        //     Destroy(platform.gameObject);
        // }
    }
    
    void Start()
    {
    }

    public float PercentSession()
    {
        //Debug.Log($"PercentSession() {GameController.Instance.ScoreManager.ScoreSession()}  {_countPlatform}");
        return (float) GameController.Instance.ScoreManager.ScoreSession() / (float)_countPlatform * 100f;
    }

    public bool EndSession()
    {
        Debug.Log($"{PercentSession()} {_countComlited}");
        return PercentSession() >= 100f;
    }

    void Update()
    {
        //EndSession();
        //Debug.Log($"{EndSession()} {_countComlited}");
        if (!_go) return;
        if (_countComlited >= _countPlatform)
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

        //if (_countComlited == _countPlatform)
        //    EventFinal?.Invoke();

        _backPlatform = Instantiate(prefabBlock, transform);
        _backPlatform.transform.position = RandomStartPosition();
        _backPlatform.SetSizeCollider(lenghtCollider);
        _backPlatform.Speed = _speed;
        _backPlatform.transform.SetParent(transform);
        RandomSizeNextCollider();
        _countComlited++;
        
       // if(_countComlited == 0) EventFirst?.Invoke();
        
        var percent = PercentSession();

        //Debug.Log($"percent:{percent}   {percent % _percentInfo}");

        if (percent % _percentInfo == 0)
            StartCoroutine(SetInfo($"{percent}% complited"));

    }

    void RandomSizeNextCollider()
    {
        _nextSizeCollider = Random.Range(prefabBlock.GetSizePlatform().z, prefabBlock.GetSizePlatform().z + 4f);
    }

    Vector3 RandomStartPosition()
    {
        return new Vector3(transform.position.x + Random.Range(-1f, 1f), transform.position.y,transform.position.z );
    }

    IEnumerator SetInfo(String txt)
    {
        _infoText.text = txt;
        yield return new WaitForSeconds(2f);
        _infoText.text = "";
    }
}
