using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPool : MonoBehaviour
{

    [SerializeField] private GameObject _daggerPrefab;
    [SerializeField] private int _initialPoolSize = 2;
    private Queue<Dagger> _daggerPool;
    private Dagger[] _daggers;
    private Transform _playerPos;

    private int _nextDaggerId = 0;

    public event Action<Vector2> Teleportation;
      
    private void Awake()
    {
        _daggerPool = new Queue<Dagger>();
        for (int i = 0; i < _initialPoolSize; i++)
        {
            CreateDagger();
        }
        GetDaggersArray();
    }

    private void CreateDagger()
    {
        GameObject daggerObject = Instantiate(_daggerPrefab);
        Dagger dagger = daggerObject.GetComponent<Dagger>();
        
        dagger.SetID(_nextDaggerId); 
        _nextDaggerId++;
        _daggerPool.Enqueue(dagger);
        daggerObject.SetActive(false);
    }
    private void GetDaggersArray()
    {
        _daggers = _daggerPool.ToArray();
    }

    private Dagger GetDagger(Vector2 targetPos)
    {
        if (_daggerPool.Count > 0)
        {
            Dagger dagger = _daggerPool.Dequeue();
            dagger.gameObject.SetActive(true);
            dagger.SetDagger(StartPos(_playerPos), targetPos);
            return dagger;
        }
       else return null;
    }
    public void OnGetDager(Vector2 targetPos)
    {
        GetDagger(targetPos);
    }
    public void Teleport(Vector2 cursorPos)
    {
        Vector2 daggerPos = CalculateNearestDaggerPos(cursorPos);
        if (daggerPos != Vector2.zero)
        {
            Teleportation?.Invoke(daggerPos);
            foreach (var dagger in _daggers)
            {
                if (dagger.gameObject.activeSelf && dagger.transform.position == (Vector3)daggerPos)
                {
                    ReturnDagger(dagger);
                    break;

                }
            }
        }
    }
    private Vector2 CalculateNearestDaggerPos(Vector2 cursorPos)
    {
        Vector2 nearestDaggerPos = Vector2.zero;
        float nearestDistance = float.MaxValue;
        foreach (var dagger in _daggers)
        {
            if (dagger.gameObject.activeSelf)
            {
                Vector2 daggerVector = new Vector2(dagger.transform.position.x, dagger.transform.position.y);
                float distance = Vector2.Distance(cursorPos, daggerVector);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestDaggerPos = daggerVector;
                }

            }
            
        }
        return nearestDaggerPos != Vector2.zero ? nearestDaggerPos : Vector2.zero;
    }
    public void ReturnDagger(Dagger dagger)
    {
        dagger.gameObject.SetActive(false);
        _daggerPool.Enqueue(dagger);
    }
    public void SetPlayerPos(Transform playerPos)
    {
        _playerPos = playerPos;
    }
    private Vector2 StartPos(Transform playerPos)
    {
        return new Vector2(playerPos.position.x, playerPos.position.y);
    }
    
}
