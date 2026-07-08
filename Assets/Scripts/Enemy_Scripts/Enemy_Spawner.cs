using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Spawner : MonoBehaviour
{
    [SerializeField] private GameObject enemySpawnIndicator;
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private float spawnInterval = 3f;

    private void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    private GameObject ChooseEnemy()
    {
        return enemyPrefabs[Random.Range(0, enemyPrefabs.Length - 1)];
    }

    private IEnumerator SpawnEnemy()
    {
        GameObject spawnIndicator = Instantiate(enemySpawnIndicator, new Vector3(Random.Range(-17f, 17f), Random.Range(-8f, 8f), 0), Quaternion.identity);

        yield return new WaitForSeconds(spawnInterval);
        GameObject enemy = Instantiate(ChooseEnemy(), spawnIndicator.transform.position, Quaternion.identity);
        Destroy(spawnIndicator);
        StartCoroutine(SpawnEnemy());
    }
}
