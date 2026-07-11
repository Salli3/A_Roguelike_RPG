using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Action_Bar : MonoBehaviour
{
    [SerializeField] private Player_Controler playerController;
    [SerializeField] private Transform player;
    [SerializeField] private Slider attackBar;
    [SerializeField] private Slider dashBar;

    private void Update()
    {
        if (player == null) return;
        transform.position = player.position;

        attackBar.maxValue = 1;
        attackBar.value = playerController.timer;

        dashBar.maxValue = Stats_Manager.instance.dashCooldown;
        dashBar.value = playerController.dashCooldownTimer;
    }

}
