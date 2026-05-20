using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("Объекты интерфейса")]
    [SerializeField] private GameObject pausePanel; 

    [Header("Звук")]
    [SerializeField] private AudioSource backgroundMusic;
    [SerializeField] private AudioClip resumeClickSound;

    private bool isPaused = false; 

    void Start()
    {
        if (pausePanel != null) pausePanel.SetActive(false);
    }

    void Update()
    {
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

    public void Resume()
    {
        if (resumeClickSound != null)
        {
            GameObject tempAudio = new GameObject("TempResumeAudio");
            AudioSource tempSource = tempAudio.AddComponent<AudioSource>();
            tempSource.clip = resumeClickSound;
            tempSource.spatialBlend = 0f;
            tempSource.Play();
            Destroy(tempAudio, resumeClickSound.length);
        }

        if (pausePanel != null) pausePanel.SetActive(false); 
        Time.timeScale = 1f;                                 
        isPaused = false;

        if (backgroundMusic != null)
        {
            backgroundMusic.UnPause();
        }
    }

    public void Pause()
    {
        if (pausePanel != null) pausePanel.SetActive(true);  
        Time.timeScale = 0f;                                 
        isPaused = true;

        if (backgroundMusic != null)
        {
            backgroundMusic.Pause();
        }
    }

    public void QuitToMenu()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(0); 
    }
}