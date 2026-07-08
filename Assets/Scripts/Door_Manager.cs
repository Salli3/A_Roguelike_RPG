using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Manager : MonoBehaviour
{
    private void OnEnable()
    {
        Enemy_HP.OnBattleEnd += OpenDoor;
    }

    private void OnDisable()
    {
        Enemy_HP.OnBattleEnd -= OpenDoor;
    }

    private void Start()
    {
        gameObject.SetActive(true);
    }

    private void OpenDoor()
    {
        gameObject.SetActive(false);
    }
}
