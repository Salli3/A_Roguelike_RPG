using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause_Menu : MonoBehaviour
{
    [SerializeField] private CanvasGroup pauseCanvasGroup;
    private bool isPaused = false;

    private void Start()
    {
        pauseCanvasGroup.alpha = 0;
        pauseCanvasGroup.interactable = false;
        pauseCanvasGroup.blocksRaycasts = false;
        Time.timeScale = 1;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                Debug.Log("Escape key pressed!");
                pauseCanvasGroup.alpha = 1;
                pauseCanvasGroup.interactable = true;
                pauseCanvasGroup.blocksRaycasts = true;
                Game_Manager.instance.PauseGame(true);
                isPaused = true;
            }
            else
            {
                pauseCanvasGroup.alpha = 0;
                pauseCanvasGroup.interactable = false;
                pauseCanvasGroup.blocksRaycasts = false;
                Game_Manager.instance.PauseGame(false);
                isPaused = false;
            }
        }
    }

    public void Resume()
    {
        pauseCanvasGroup.alpha = 0;
        pauseCanvasGroup.interactable = false;
        pauseCanvasGroup.blocksRaycasts = false;
        Game_Manager.instance.PauseGame(false);
        isPaused = false;
    }

    public void Quit()
    {
        Game_Manager.instance.CleanUpAndDestroy();
        SceneManager.LoadScene("MainMenu");
    }
}
