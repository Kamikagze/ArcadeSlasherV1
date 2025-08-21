using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Cursor : MonoBehaviour
{

    public event Action<Vector2> CursorPosition ;
    
    // Ссылка на камеру (можно задать в инспекторе). 
    // Если не указана — в Start() подхватит Camera.main
    public Camera cam;
    private WeaponPool _pool;

    private bool _isPossibleToTeleport = true;
    void Start()
    {
        if (cam == null)
            cam = Camera.main;
    }

    void Update()
    {
        // 1. Захватываем позицию мыши в пикселях
        Vector3 screenPos = Input.mousePosition;

        // 2. Привязываем к границам окна
        screenPos.x = Mathf.Clamp(screenPos.x, 0f, Screen.width);
        screenPos.y = Mathf.Clamp(screenPos.y, 0f, Screen.height);

        // 3. Задаём дистанцию до камеры, чтобы ScreenToWorldPoint правильно вернул z=0
        //    (для ортографической камеры z не влияет на x,y, но влияет на выходной z)
        screenPos.z = -cam.transform.position.z;

        // 4. Переводим экранную позицию в мировую
        Vector3 worldPos = cam.ScreenToWorldPoint(screenPos);

        // 5. Перемещаем объект-курсор
        transform.position = new Vector3(worldPos.x, worldPos.y, 0f);

        // 6. При клике правой кнопкой отправляем координаты
        if (Input.GetMouseButtonDown(1))
        {
            SendPosition(CursorPosIn2(worldPos));
        }
        if (Input.GetKey(KeyCode.Q) && _isPossibleToTeleport)
        {
            _pool.Teleport(worldPos);
            StartCoroutine(TeleportCoroutine());
        }
    }

    private IEnumerator TeleportCoroutine()
    {
        _isPossibleToTeleport = false;
        yield return new WaitForSeconds(0.2f);
        _isPossibleToTeleport = true ;
        StopCoroutine(TeleportCoroutine());
    }
    // Метод для отправки или обработки позиции курсора
    public void SendPosition(Vector2 pos)
    {
        CursorPosition?.Invoke(pos);
    }
    public void SetWeaponPool(WeaponPool weaponPool)
    {
        _pool = weaponPool;
    }
    private Vector2 CursorPosIn2(Vector3 cursorPos)
    {
        float x = cursorPos.x;
        float y = cursorPos.y;
        return new Vector2(x, y);
    }
    //public void SetEvent (Action<Vector2> action)
    //{
    //    CursorPosition = action;
    //}
}

