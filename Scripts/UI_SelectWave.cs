using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_SelectWave : MonoBehaviour
{
    public TextMeshProUGUI textLevel;

    private WaveManager waveManager;
    private GameManager gameManager;

    public void Init(GameManager gameManager, WaveManager waveManager)
    {
        this.gameManager = gameManager;
        this.waveManager = waveManager;
    }

    private void OnEnable()
    {
        UpdateVisual();
    }

    public void SetNextLevel()
    {
        waveManager.SetNextLevel();
        UpdateVisual();
    }

    public void SetPreviousLevel()
    {
        waveManager.SetPreviousLevel();
        UpdateVisual();
    }

    public void StartWave()
    {
        gameManager.StartGame();
    }

    private void UpdateVisual()
    {
        Debug.Log(waveManager.MaxLevel);
        textLevel.SetText(waveManager.level.ToString());
    }
}
