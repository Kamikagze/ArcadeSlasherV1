using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _player; 
    [SerializeField] private float smoothSpeed = 0.125f; 
    [SerializeField] private Vector3 offset; 
    [SerializeField] private float deadZoneWidth = 0.5f; 
    [SerializeField] private float deadZoneHeight = 0.5f; 

    private void LateUpdate()
    {
       if (_player != null )
        {
            Vector3 targetPosition = _player.position + offset;


            Vector3 currentPosition = transform.position;


            if (Mathf.Abs(currentPosition.x - targetPosition.x) > deadZoneWidth ||
                Mathf.Abs(currentPosition.y - targetPosition.y) > deadZoneHeight)
            {

                Vector3 newPosition = Vector3.Lerp(currentPosition, targetPosition, smoothSpeed);
                transform.position = newPosition;
            }
        }
        
    }
    private void OnDrawGizmos()
    {
        // Проверяем, установлен ли игрок
        if (_player != null)
        {
            // Отрисовка зоны неподвижности
            Gizmos.color = Color.red; // Цвет для зоны неподвижности
            Vector3 playerPosition = _player.position + offset;
            Vector3 center = playerPosition;

            // Параметры прямоугольника зоны неподвижности
            Vector3 size = new Vector3(deadZoneWidth * 2, deadZoneHeight * 2, 0);

            // Отрисовка прямоугольника Gizmos
            Gizmos.DrawWireCube(center, size);
        }
    } ////
    public void SetPlayer(Transform player)
    {
        _player = player;
    }
    
}
