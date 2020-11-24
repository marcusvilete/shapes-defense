using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public GameObject winUI;
    public GameObject loseUI;
    public int startingCurrency = 0;
    public WaveManager waveManager;
    public HomeBase homeBase;

    //this is so we can determine when level is over(win)
    private int enemyCounter = 0;
    public bool lastWaveSpawned;

    void Start()
    {
        Time.timeScale = 1f;
        CurrencyManager.Instance.Initialize(startingCurrency);
        waveManager.waveCompleted += WaveManager_waveCompleted;
        waveManager.enemySpawned += WaveManager_enemySpawned;
        homeBase.OnDamageTaken += HomeBase_OnDamageTaken;   
    }

    //Keeping track of home base health, so we can call it a lose if it reaches Zero
    private void HomeBase_OnDamageTaken(HealthChangedData data)
    {
        if (data.newHealth <= 0)
        {
            LoseLevel();
        }
    }

    //Keeping track of enemies alive, so we can call it a win 
    private void WaveManager_enemySpawned(Enemy spawned)
    {
        spawned.OnDeath += Enemy_OnDeath;
        enemyCounter++;
    }

    //Keeping track of enemies alive, so we can call it a win
    private void Enemy_OnDeath(Enemy dead)
    {
        dead.OnDeath -= Enemy_OnDeath;
        enemyCounter--;

        if (lastWaveSpawned && enemyCounter < 1)
        {
            WinLevel();
        }
    }

    //Keeping track of waves, so we can call it a win
    private void WaveManager_waveCompleted(bool isLastWave)
    {
        lastWaveSpawned = isLastWave;
    }


    //Something something, we won
    private void WinLevel()
    {
        Time.timeScale = 0f;
        loseUI.SetActive(false);
        winUI.SetActive(true);
    }

    //Something something, lost the game =.(
    private void LoseLevel()
    {
        Time.timeScale = 0f;
        winUI.SetActive(false);
        loseUI.SetActive(true);
    }

    public void PlayAgain()
    {
        GameManager.Instance.LoadLevel1();
    }

    public void BackToMenu()
    {
        GameManager.Instance.LoadStartMenu();
    }
}
