using System;
using UnityEngine;

public class SlimeTracker : MonoBehaviour
{
    public static SlimeTracker Instance { get; private set; }

    public event EventHandler OnAllSlimesKilled;

    [Header("Количество слаймов на уровне")]
    [SerializeField] private int totalSlimes = 24;

    private int _killedSlimes = 0;
    private bool _allKilled = false;

    public bool AllSlimesKilled => _allKilled;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void RegisterSlimeDeath()
    {
        if (_allKilled) return;

        _killedSlimes++;
        Debug.Log($"Слаймов убито: {_killedSlimes}/{totalSlimes}");

        if (_killedSlimes >= totalSlimes)
        {
            _allKilled = true;
            OnAllSlimesKilled?.Invoke(this, EventArgs.Empty);
        }
    }
}