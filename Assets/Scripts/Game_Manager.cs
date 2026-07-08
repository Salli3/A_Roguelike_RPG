using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    public static Game_Manager instance;

    public Boss_UI bossUI;
    public int bossOnScreen = 0;
    [SerializeField] private float difficultylevel = 10f;
    [SerializeField] private float difficultyMultiplier = 1.2f;
    [SerializeField] private int currentStage = 1;
    [SerializeField] private int bossStageInterval = 5;

    [Header("Persistent Objects")]
    public GameObject[] persistentObjects;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            MarkPersistentObjects();
        }
        else
        {
            CleanUpAndDestroy();
            return;
        }
    }

    private void MarkPersistentObjects()
    {
        foreach (GameObject obj in persistentObjects)
        {
            if (obj != null)
            {
                DontDestroyOnLoad(obj);
            }
        }
    }

    private void CleanUpAndDestroy()
    {
        foreach (GameObject obj in persistentObjects)
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }

        Destroy(gameObject);
    }

    public void IncreaseDifficulty()
    {
        difficultylevel *= difficultyMultiplier;
        currentStage++;
    }

    public float GetDifficultyLevel()
    {
        return difficultylevel;
    }

    public bool IsBossStage()
    {
        return currentStage % bossStageInterval == 0;
    }
}
