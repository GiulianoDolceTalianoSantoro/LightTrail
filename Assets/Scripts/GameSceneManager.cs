using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using yaSingleton;

[CreateAssetMenu(fileName = "GameSceneManager", menuName = "Systems/GameSceneManager")]
public class GameSceneManager : Singleton<GameSceneManager>
{
    public GameObject pauseMenuPrefab;
    private bool pauseMenuIsActive = false;
    public GameObject canvasPrefab;


    private Canvas _canvas;
    public Canvas Canvas
    {
        get { 
            // Check if canvas variable is set
            if (_canvas == null)
            {
                // If not set, look in scene for canvas
                _canvas = FindObjectOfType<Canvas>();

                if(_canvas == null)
                {
                    // If no canvas in scene, create one
                    Instantiate(canvasPrefab).GetComponent<Canvas>();
                }
            }

            return _canvas;
        }
    }

    private GameObject _pauseMenuInstance;

    public void OpenPauseMenu()
    {
        if (_pauseMenuInstance == null)
        {
            // Creates a new pause menu
            Instantiate(pauseMenuPrefab, Canvas.transform);
        }
        else
        {
            // Set currently existing pause menu as active
            pauseMenuIsActive = true;
            _pauseMenuInstance.SetActive(true);
        }
    }

    public void ClosePauseMenu()
    {
        pauseMenuIsActive = false;
        _pauseMenuInstance.SetActive(false);
    }

    public override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pauseMenuIsActive)
            {
                OpenPauseMenu();
            }
            else
            {
                ClosePauseMenu();
            }
        }
    }
}
