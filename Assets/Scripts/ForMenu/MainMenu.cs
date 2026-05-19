using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Нужен для работы с ползунком

public class MainMenu : MonoBehaviour
{
    [Header("Панели меню")]
    [SerializeField] private GameObject settingsPanel; // Ссылка на панель настроек
     [SerializeField] private GameObject aboutPanel; // Ссылка на панель "Инструкция"
  
    [SerializeField] private GameObject instructionPanel; // Ссылка на Instruction
   
    [Header("Настройки звука")]
    [SerializeField] private Slider volumeSlider;     // Ссылка на ползунок

    private void Start()
    {
        // При старте игры проверяем, сохранен ли звук, и выставляем ползунок
        if (volumeSlider != null)
        {
            volumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
        
        // На всякий случай скрываем настройки при запуске
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
        if (instructionPanel != null) instructionPanel.SetActive(false);
        if (aboutPanel != null) aboutPanel.SetActive(false);
    }
     // Метод для открытия панели "О игре"
    public void OpenAbout()
    {
        if (aboutPanel != null) aboutPanel.SetActive(true);
    }

    // Метод для закрытия панели "О игре"
    public void CloseAbout()
    {
        if (aboutPanel != null) aboutPanel.SetActive(false);
    }

      // --- ИНСТРУКЦИЯ (Как играть) ---
    public void OpenInstruction() {
         if (instructionPanel != null) instructionPanel.SetActive(true); }
    public void CloseInstruction() {
         if (instructionPanel != null) instructionPanel.SetActive(false); }


    public void PlayGame()
    {
        SceneManager.LoadScene(1); 
    }

    // Метод для открытия панели настроек
    public void OpenSettings()
    {
        if (settingsPanel != null) settingsPanel.SetActive(true);
    }

    // Метод для закрытия панели настроек
    public void CloseSettings()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);
    }

    // Метод для изменения громкости
    public void SetVolume(float value)
    {
        // Регулируем общую громкость приложения (от 0.0 до 1.0)
        AudioListener.volume = value;

        // Сохраняем настройку в память устройства, чтобы она не сбрасывалась при перезапуске
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

        // Метод для выхода из игры
    public void ExitGame()
    {
        // Выводит сообщение в консоль редактора Unity (для проверки во время разработки)
        Debug.Log("Игрок нажал кнопку ВЫХОД");

        // Закрывает приложение (работает в скомпилированной игре на ПК или телефоне)
        Application.Quit();
    }

}
