using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

class MultiplayerMenu : MonoBehaviour
{
    public GameObject mainMenu;

    public void Enter()
    {
        mainMenu.SetActive(false);
        gameObject.SetActive(true);
    }

    public void Exit()
    {
        gameObject.SetActive(false);
        mainMenu.SetActive(true);
    }
}
