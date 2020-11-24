using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{

    public List<Wave> waves = new List<Wave>();
    public Transform[] waypoints;
    public bool shouldStartImmediately;
    int currentWaveIndex;

    public float waitBeforeSpawn;
    public event Action<bool> waveCompleted;
    public event Action<Enemy> enemySpawned;


    IEnumerator Start()
    {
        if (shouldStartImmediately)
        {
            yield return new WaitForSeconds(waitBeforeSpawn);
            StartWave();
        }
    }

    public void StartWave()
    {
        if (waves.Count > 0 && currentWaveIndex < waves.Count)
        {
            Wave wave = waves[currentWaveIndex];
            wave.waveCompleted += Wave_waveCompleted;
            wave.enemySpawned += Wave_enemySpawned;
            wave.StartSpawning(waypoints);
        }
    }

    private void Wave_enemySpawned(Enemy spawned)
    {
        //bubble up
        enemySpawned?.Invoke(spawned);
    }

    private void Wave_waveCompleted()
    {
        //bubble up
        //param is: was last wave?
        waveCompleted?.Invoke(currentWaveIndex + 1 >= waves.Count);
    }

    public void NextWave()
    {
        //cleanup
        waves[currentWaveIndex].waveCompleted -= Wave_waveCompleted;
        currentWaveIndex++;
        StartWave();
    }
}

