using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterPoint : MonoBehaviour
{
    [SerializeField] GameObject _cursorGO;
    [SerializeField] GameObject _weaponPoolGO;
    [SerializeField] GameObject _playerGO;

    private Cursor _cursor;
    private WeaponPool _weaponPool;
    private PLController _player;
    private void Awake()
    {
        CreateCursor();
        
        CreateWeaponPool();
        CreatePlayer();
        GetNessesaryValyes();

    }
    private void CreateCursor()
    {
        GameObject cursorObject = Instantiate(_cursorGO);
        _cursor = cursorObject.GetComponent<Cursor>();        
    }
    private void CreateWeaponPool()
    {
        GameObject weaponPoolGameObject = Instantiate(_weaponPoolGO);
        _weaponPool = weaponPoolGameObject.GetComponent<WeaponPool>();
    }
    private void CreatePlayer()
    {
        GameObject playerGameObject = Instantiate(_playerGO);
        _player = playerGameObject.GetComponent<PLController>();
    }
    
    private void GetCamera()
    {
        CameraController camera = FindObjectOfType<CameraController>();
        camera.SetPlayer(_player.transform);
    }
    private void GetNessesaryValyes()
    {
        GetCamera();
        _weaponPool.SetPlayerPos(_player.transform);
        _cursor.SetWeaponPool(_weaponPool);
        _cursor.CursorPosition += _weaponPool.OnGetDager;
        _weaponPool.Teleportation += _player.OnTeleportation;
        
    }
}
