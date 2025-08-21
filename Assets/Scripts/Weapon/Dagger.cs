using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger : MonoBehaviour
{
    private int ID;
    
    private Vector2 _targetPos;
    private Vector2 _startPos;
    [SerializeField] float _daggerSpeed;
    private Rigidbody2D _daggerRB;
    private BoxCollider2D _daggerCollider;

    public event Action<Vector2> DaggerTeleport;
    private void Awake()
    {
        _daggerCollider = GetComponent<BoxCollider2D>();
        _daggerRB = GetComponent<Rigidbody2D>();
    }
    public void SetID(int id)
    {
        ID = id;
    }
    
    public void SetDagger(Vector2 startPos, Vector2 targetPos)
    {
        _startPos = startPos;
        _targetPos = targetPos;
        
        transform.position = new Vector3(startPos.x, startPos.y, 0);
        Vector2 direction = (targetPos - startPos).normalized;
        SetDaggerOrientation(direction, startPos, targetPos);

        ThrowDagger(direction);
    }
    private void SetDaggerOrientation(Vector2 direction, Vector2 startPos, Vector2 targetPos)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
    private void ThrowDagger(Vector2 direction)
    {
        _daggerRB.velocity = direction * _daggerSpeed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            _daggerRB.velocity = Vector2.zero;
        }

    }

    //public void OnDaggerTeleport()
    //{
    //    Vector2 daggerPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
    //    DaggerTeleport?.Invoke(daggerPos);
    //}
}
