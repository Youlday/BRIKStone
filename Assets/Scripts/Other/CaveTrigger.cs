using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CaveTrigger : MonoBehaviour
{
    [Header("UI элементы")]
    [SerializeField] private CanvasGroup endScreenCanvasGroup; 
    [SerializeField] private float fadeDuration = 1.5f;        
    [SerializeField] private float delayBeforeShow = 0.5f;     

    [Header("Звук")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip winSound;

    private bool _triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_triggered) return;
        if (!other.TryGetComponent(out Player player)) return;

        if (SlimeTracker.Instance == null || !SlimeTracker.Instance.AllSlimesKilled)
        {
            Debug.Log("Ещё не все слаймы убиты!");
            return;
        }

        _triggered = true;

        if (audioSource != null && winSound != null)
        {
            audioSource.PlayOneShot(winSound);
        }

        KillEnemies();
        Player.Instance.DisableMovement();
        StartCoroutine(ShowEndScreen());
    }

    private void KillEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
    }

    private IEnumerator ShowEndScreen()
    {
        yield return new WaitForSeconds(delayBeforeShow);
        
        float elapsed = 0f;
        endScreenCanvasGroup.alpha = 0f;
        endScreenCanvasGroup.gameObject.SetActive(true);

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            endScreenCanvasGroup.alpha = Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }

        endScreenCanvasGroup.alpha = 1f;
    }
}