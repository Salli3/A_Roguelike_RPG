using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player_HP : MonoBehaviour
{
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private Animator hpBarAnim;
    [SerializeField] private Slider hpBar;
    public static event Action OnPlayerDefeated;

    [SerializeField] private Camera mainCamera;
    private Vector3 originalPos;
    private Coroutine shakeRoutine;

    [SerializeField] private SpriteRenderer currentSprite;

    [SerializeField] private Player_Controler playerControler;

    private void Start()
    {
        hpText.text = Mathf.CeilToInt(Stats_Manager.instance.currentHP) + "/" + Mathf.CeilToInt(Stats_Manager.instance.maxHP);
        currentSprite.sprite = Stats_Manager.instance.playerSprite;
        UpdateUI();
    }

    public void ChangeHP(float amount)
    {
        if (playerControler.isDashing) return;
        Stats_Manager.instance.currentHP -= amount;
        hpBarAnim.Play("Update");
        hpText.text = Mathf.CeilToInt(Stats_Manager.instance.currentHP) + "/" + Mathf.CeilToInt(Stats_Manager.instance.maxHP);

        if (Stats_Manager.instance.currentHP <= 0)
        {
            OnPlayerDefeated?.Invoke();
            gameObject.SetActive(false);
        }
        if (gameObject.activeSelf)
        {
            Shake();
        }
        UpdateUI();
    }

    private void UpdateUI()
    {
        hpBar.maxValue = Stats_Manager.instance.maxHP;
        hpBar.value = Stats_Manager.instance.currentHP;
    }

    public void Shake(float duration = 0.2f, float magnitude = 0.3f)
    {
        if (shakeRoutine != null)
            StopCoroutine(shakeRoutine);

        shakeRoutine = StartCoroutine(DoShake(duration, magnitude));
    }

    private IEnumerator DoShake(float duration, float magnitude)
    {
        currentSprite.sprite = Stats_Manager.instance.playerHitSprite;
        originalPos = mainCamera.transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = UnityEngine.Random.Range(-1f, 1f) * magnitude;
            float y = UnityEngine.Random.Range(-1f, 1f) * magnitude;

            mainCamera.transform.localPosition = originalPos + new Vector3(x, y, 0f);

            elapsed += Time.unscaledDeltaTime; // works even if Time.timeScale = 0
            yield return null;
        }

        mainCamera.transform.localPosition = originalPos;
        shakeRoutine = null;
        currentSprite.sprite = Stats_Manager.instance.playerSprite;
    }
}
