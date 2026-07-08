using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Spawner : MonoBehaviour
{
    [SerializeField] private Transform enemySpawnerTransform;
    [SerializeField] private GameObject enemySpawnIndicator;
    [SerializeField] private GameObject[] normalEnemyPrefabs;
    [SerializeField] private GameObject[] mixEnemyPrefabs;
    [SerializeField] private GameObject[] bossPrefabs;
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private float difficultyLevel;
    [SerializeField] private bool isBossStage;
    [SerializeField] private bool isBossChosen = false;

    private void Start()
    {
        difficultyLevel = Game_Manager.instance.GetDifficultyLevel();
        isBossStage = Game_Manager.instance.IsBossStage();
    }

    private void Update()
    {
        if (isBossStage && difficultyLevel > 1 && isBossChosen == false)
        {
            StartCoroutine(SpawnEnemy(ChooseEnemy(bossPrefabs)));
            isBossChosen = true;
        }
        else if (isBossStage && difficultyLevel > 1)
        {
            StartCoroutine(SpawnEnemy(ChooseEnemy(mixEnemyPrefabs)));
        }
        else if (difficultyLevel > 1)
        {
            StartCoroutine(SpawnEnemy(ChooseEnemy(normalEnemyPrefabs)));
        }
    }

    private GameObject ChooseEnemy(GameObject[] availableEnemy)
    {
        List<GameObject> affordable = new List<GameObject>();
        foreach (var prefab in availableEnemy)
        {
            if (prefab.GetComponent<Enemy_HP>().enemySO.dangerLevel <= difficultyLevel)
            {
                affordable.Add(prefab);
            }
        }

        if (affordable.Count == 0)
        {
            //Nothing fits anymore — force loop to end by draining remaining budget
            difficultyLevel = 0;
            return normalEnemyPrefabs[0]; // fallback, won't actually be used since loop will exit
        }

        GameObject chosen = affordable[Random.Range(0, affordable.Count)];
        difficultyLevel -= chosen.GetComponent<Enemy_HP>().enemySO.dangerLevel;
        return chosen;
    }

    private IEnumerator SpawnEnemy(GameObject enemy)
    {
        GameObject spawnIndicator = Instantiate(enemySpawnIndicator, new Vector3(Random.Range(-17f, 17f), Random.Range(-8f, 8f), 0), Quaternion.identity, enemySpawnerTransform);

        yield return new WaitForSeconds(spawnInterval);
        GameObject newEnemy = Instantiate(enemy, spawnIndicator.transform.position, Quaternion.identity);
        Destroy(spawnIndicator);
    }
}
