using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    public static Game_Manager instance;

    public Boss_UI bossUI;
    public int bossOnScreen = 0;
    public int enemyOnScreen = 0;
    public int uiOnScreen = 0;
    [SerializeField] private float difficultylevel = 10f;
    [SerializeField] private float difficultyMultiplier = 1.2f;
    [SerializeField] public int currentStage = 0;
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

    public void PauseGame(bool isPaused)
    {
        if (isPaused)
        {
            uiOnScreen++;
        }
        else
        {
            uiOnScreen--;
        }

        if (uiOnScreen == 0)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
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

    public void CleanUpAndDestroy()
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
