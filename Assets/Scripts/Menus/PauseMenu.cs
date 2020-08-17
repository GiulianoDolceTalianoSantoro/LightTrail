using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject saveMenuPrefab;
    public GameSceneManager gameSceneManager;

    public void CloseMenu()
    {
        Destroy(this.gameObject);
    }

    public void OpenMainMenu()
    {

    }
}
