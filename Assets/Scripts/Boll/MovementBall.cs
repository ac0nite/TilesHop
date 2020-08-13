using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TreeEditor;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class MovementBall : MonoBehaviour
{
    private bool _down = false;
    private float _firstTap = 0f;
    [SerializeField] private float _maxDownPosition = 4f;
    [SerializeField] private float _minDownBall = -1f;
    [SerializeField] private Vector3 _defaultPosition = Vector3.zero;
    [SerializeField] private float _defaultSpeed = 8f;
    [SerializeField] private Vector3 _defaultTargetFlyingBall = Vector3.zero;
    [SerializeField] private Transform _trail = null;
    private Vector3 _downPosition = Vector3.down;
    private Vector3 _upPosition = Vector3.zero;
    private float _movementSpeed = 8f;
    private Vector3 _targetPosition = Vector3.zero;
    private bool go = false;

    Vector3 firstVec = Vector3.zero;
    public event Action EventAddScore;
    public event Action EventStop;
    public event Action EventPassed;

    private void Awake()
    {
        GameController.Instance.EventGo += OnGo;
        GameController.Instance.TilesController.EventFinal += OnFinal;
    }

    private void OnDestroy()
    {
        GameController.Instance.EventGo -= OnGo;
        GameController.Instance.TilesController.EventFinal -= OnFinal;
    }

    private void OnGo()
    {
        go = true;
        _upPosition = new Vector3(transform.position.x, _maxDownPosition, transform.position.z);
        _downPosition = new Vector3(transform.position.x, _minDownBall, transform.position.z);
        _targetPosition = _upPosition;
        _movementSpeed = _defaultSpeed;
    }

    void Start()
    {
    }

    public void DefaultPosition()
    {
        transform.position = _defaultPosition;
        _targetPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        var l = other.transform.position.z - transform.position.z;
        var t = l / other.GetComponent<MovementTile>().Speed;
        var v = transform.position.y / t;
        _movementSpeed = v;
        _targetPosition = new Vector3(transform.position.x, _downPosition.y, transform.position.z);
    }

    private void OnCollisionEnter(Collision other)
    {
        
        _targetPosition = new Vector3(transform.position.x, _upPosition.y, transform.position.z);
        EventAddScore?.Invoke();
    }

    private void OnFinal()
    {
        Debug.Log($"OnFinal");
        StartCoroutine(FlyingBallCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        if(!go) return;


        if (transform.position.y <= _minDownBall || Input.GetMouseButtonUp(0))
        {
            go = false;
            EventStop?.Invoke();
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            _down = true;
            _firstTap = Input.mousePosition.x;
            firstVec =  Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        }

        if (Input.GetMouseButtonUp(0))
        {
            _down = false;
        }

        if (Input.GetMouseButton(0))
        {    
            var frw = Input.mousePosition.x - _firstTap;
            _firstTap = Input.mousePosition.x;

           var m = Input.mousePosition;
           var p = Camera.main.ScreenToWorldPoint(new Vector3(m.x, m.y, Camera.main.nearClipPlane));
           
                 float tmp = 0f;
                 if (frw > 0f)
                 {
                     tmp = _targetPosition.x + ((p - firstVec).magnitude * 80);
                 }
                 else
                 {
                     tmp  = _targetPosition.x - ((p - firstVec).magnitude * 80);
                 }
                 firstVec = p;
                 var viewportPoint = Camera.main.WorldToViewportPoint(new Vector3(tmp, transform.position.y, transform.position.z));
                 if (viewportPoint.x > 0f && viewportPoint.x < 1f)
                 {
                     _targetPosition.x = tmp;
                 }
        }

        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _movementSpeed * Time.deltaTime);
    }

    IEnumerator FlyingBallCoroutine()
    {
        _trail.gameObject.SetActive(true);
        _targetPosition = _defaultTargetFlyingBall;
        _movementSpeed = 40f;
        yield return new WaitForSeconds(2f);
        EventPassed?.Invoke();
        _trail.gameObject.SetActive(false);
    }
}
