using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("Объекты интерфейса")]
    [SerializeField] private GameObject pausePanel; // Ссылка на панель паузы

    private bool isPaused = false; // Состояние игры

    void Start()
    {
        // При старте уровня панель паузы гарантированно скрыта
        if (pausePanel != null) pausePanel.SetActive(false);
    }

    void Update()
    {
        // Отслеживаем нажатие клавиши Esc (Escape)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    // Метод для снятия с паузы (Продолжить)
    public void Resume()
    {
        if (pausePanel != null) pausePanel.SetActive(false); // Прячем панель
        Time.timeScale = 1f;                                 // Запускаем время
        isPaused = false;
    }

    // Метод для включения паузы
    public void Pause()
    {
        if (pausePanel != null) pausePanel.SetActive(true);  // Показываем панель
        Time.timeScale = 0f;                                 // Замораживаем время (все застынут)
        isPaused = true;
    }

    // Метод для выхода в главное меню
    public void QuitToMenu()
    {
        Time.timeScale = 1f; // КРИТИЧНО: возвращаем время в норму, иначе главное меню тоже замрет!
        SceneManager.LoadScene(0); // Загружаем сцену меню (индекс 0 в Build Settings)
    }
}