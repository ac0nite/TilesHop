using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTile : MonoBehaviour
{

    [SerializeField] private float _speed = 1f;
    private Transform _transform = null;
    private BoxCollider _boxCollider = null;
    [SerializeField] private Transform _platform = null;
    private Vector3 _targetPosition = Vector3.zero;
    private TilesController _tilesController = null;
    private bool _movement = true;
    public bool Completion { get; private set; }

    public float Speed
    {
        get { return _speed; }
        set { _speed = value; }
    }

    private void Awake()
    {
            Completion = false;
        _transform = GetComponent<Transform>();
        _boxCollider = GetComponent<BoxCollider>();
        _tilesController = GetComponentInParent<TilesController>();

        _tilesController.EventMovement += OnMovement;
    }

    private void OnDestroy()
    {
        _tilesController.EventMovement -= OnMovement;
    }

    private void OnMovement(bool movement)
    {
        _movement = movement;
    }

    public Vector3 GetSizeCollider()
    {
        return _boxCollider.size;
    }

    public Vector3 GetSizePlatform()
    {
        return _platform.localScale;
    }
    
    public void SetSizeCollider(float sizeLength)
    {
        _boxCollider.size = new Vector3(_boxCollider.size.x, _boxCollider.size.y, Mathf.Clamp(sizeLength, _platform.localScale.z, float.MaxValue));
    }
    void Start()
    {
        _targetPosition = new Vector3(transform.position.x, 0f, 0f);
    }
    
    void Update()
    {
        if(!_movement) return;
        _transform.position = Vector3.MoveTowards(_transform.position, _targetPosition, _speed * Time.deltaTime);
        if (_transform.position.z <= 0f)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnCollisionExit(Collision other)
    {
        Completion = true;
    }
}
