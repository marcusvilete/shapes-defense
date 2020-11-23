using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveUIController : MonoBehaviour
{
    WaveManager waveManager;

    public Button readyButton;
    public Button nextWaveButton;

    public void NextWaveClick()
    {
        nextWaveButton.interactable = false;
        waveManager.NextWave();
    }

    public void ReadyClick()
    {
        readyButton.interactable = false;
        waveManager.StartWave();
    }

    void Start()
    {
        waveManager = FindObjectOfType<WaveManager>();
        waveManager.waveCompleted += WaveManager_waveCompleted;
    }

    private void WaveManager_waveCompleted(bool isLastWave)
    {
        if (!isLastWave)
        {
            nextWaveButton.interactable = true;
        }
    }
}
