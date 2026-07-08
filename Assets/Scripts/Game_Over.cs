using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Over : MonoBehaviour
{
    [SerializeField] private CanvasGroup gameOverCanvas;
    [SerializeField] private CanvasGroup optionsCanvas;
    [SerializeField] private Animator anim;
    [SerializeField] private TMP_Text scoreText;

    private void OnEnable()
    {
        Player_HP.OnPlayerDefeated += DisplayGameOverScreen;
    }

    private void OnDisable()
    {
        Player_HP.OnPlayerDefeated -= DisplayGameOverScreen;
    }

    private void Start()
    {
        gameOverCanvas.alpha = 0;
        gameOverCanvas.interactable = false;
        gameOverCanvas.blocksRaycasts = false;
        optionsCanvas.alpha = 0;
        optionsCanvas.interactable = false;
        optionsCanvas.blocksRaycasts = false;
    }
    private void DisplayGameOverScreen()
    {
        gameOverCanvas.alpha = 1;
        scoreText.text = "Score: " + Game_Manager.instance.currentStage;
        anim.Play("Show");
        StartCoroutine(ShowOption());
    }

    private IEnumerator ShowOption()
    {
        yield return new WaitForSeconds(1);
        optionsCanvas.alpha = 1;
        optionsCanvas.interactable = true;
        optionsCanvas.blocksRaycasts = true;
    }

    public void NewGame()
    {
        Game_Manager.instance.CleanUpAndDestroy();
        SceneManager.LoadScene("Start");
    }
    public void MainMenu()
    {
        Game_Manager.instance.CleanUpAndDestroy();
        SceneManager.LoadScene("MainMenu");
    }
    public void QuitGame()
    {
        Debug.Log("Quit game");
        Application.Quit();
    }
}
