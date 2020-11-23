using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{

    public event Action waveCompleted;
    public event Action<Enemy> enemySpawned;
    public List<SpawnData> spawnData;
    Transform[] waypoints;




    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void StartSpawning(Transform[] waypoints)
    {
        this.waypoints = waypoints;
        StartCoroutine(SpawnCoroutine());
    }


    IEnumerator SpawnCoroutine()
    {
        foreach (var spawnItem in spawnData)
        {
            yield return new WaitForSeconds(spawnItem.waitTimeBeforeSpawn);
            var spawned = Instantiate(spawnItem.toSpawn, spawnItem.spawnPoint.position, spawnItem.spawnPoint.rotation);
            spawned.SetWayPoints(waypoints);
            enemySpawned?.Invoke(spawned);
        }
        waveCompleted?.Invoke();
    }

}
