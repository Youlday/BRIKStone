using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapController : MonoBehaviour
{
    [Header("Объекты интерфейса")]
    public GameObject smallMapUI; 
    public GameObject bigMapUI;   

    [Header("Настройки клавиш")]
    public KeyCode mapKey = KeyCode.M; 

    [Header("Камера карты")]
    public Camera minimapCamera;   
    public float smallMapSize = 10f; 
    public float bigMapSize = 35f;   

    [Header("Настройки центра локации")]
    [Tooltip("Координаты центра вашей игровой карты")]
    public Vector2 mapCenterCoordinates = Vector2.zero; // Сюда в инспекторе введем центр карты
    
    private Transform playerTransform; // Ссылка для возврата камеры к игроку

    void Start()
    {
        if (bigMapUI != null) bigMapUI.SetActive(false);
        if (smallMapUI != null) smallMapUI.SetActive(true);

        if (minimapCamera != null)
        {
            minimapCamera.orthographicSize = smallMapSize;
            // Запоминаем трансформ игрока (так как камера изначально дочерняя ему)
            playerTransform = minimapCamera.transform.parent;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(mapKey))
        {
            ToggleMaps();
        }
    }

    void ToggleMaps()
    {
        if (bigMapUI != null && smallMapUI != null && minimapCamera != null)
        {
            bool isBigMapOpen = bigMapUI.activeSelf;

            bigMapUI.SetActive(!isBigMapOpen);   
            smallMapUI.SetActive(isBigMapOpen);  
      
            if (!isBigMapOpen) // Если ОТКРЫВАЕМ большую карту
            {
                minimapCamera.orthographicSize = bigMapSize; 

                // 1. Отвязываем камеру от игрока, чтобы она не двигалась за ним
                minimapCamera.transform.SetParent(null);

                // 2. Перемещаем камеру строго в центр игровой локации (высоту Y сохраняем прежней)
                minimapCamera.transform.position = new Vector3(mapCenterCoordinates.x, mapCenterCoordinates.y, minimapCamera.transform.position.z);

                Time.timeScale = 0f;
            }
            else // Если ЗАКРЫВАЕМ большую карту и возвращаемся в игру
            {
                minimapCamera.orthographicSize = smallMapSize; 

                // 1. Возвращаем камеру обратно «внутрь» игрока
                if (playerTransform != null)
                {
                    minimapCamera.transform.SetParent(playerTransform);
                    
                    // 2. Сбрасываем локальные координаты в 0, чтобы она снова встала ровно над головой
                    minimapCamera.transform.localPosition = new Vector3(0, 0, minimapCamera.transform.localPosition.z);
                }

                Time.timeScale = 1f; 
            }
        }
    }
}