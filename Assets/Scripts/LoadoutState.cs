using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LoadoutState : AState
{
    public Canvas loadoutCanvas;

    [Header("UI")]
    public GameObject mainMenu;
    public GameObject levelMenu;

    protected GameObject character;
    protected const float characterRotationSpeed = 45f;

    public override void Enter(AState from)
    {
        if (FindObjectOfType<RoundPlayerController>())
            Destroy(FindObjectOfType<RoundPlayerController>().gameObject);
        Time.timeScale = 1f;

        manager.loadoutCamera.gameObject.SetActive(true);
        loadoutCanvas.gameObject.SetActive(true);

        mainMenu.gameObject.SetActive(true);
        levelMenu.gameObject.SetActive(false);
    }

    public override void Exit(AState to)
    {
        loadoutCanvas.gameObject.SetActive(false);

        // Free resources
        //if (character != null) Addressables.ReleaseInstance(character);
    }

    public override string GetName()
    {
        return "Loadout";
    }

    public void StartGame()
    {
        manager.indexLevelToLoad = int.Parse(EventSystem.current.currentSelectedGameObject.name);

        manager.SwitchState("Game");
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    public override void Tick()
    {
        if (character != null)
        {
            character.transform.Rotate(0, characterRotationSpeed * Time.deltaTime, 0, Space.Self);
        }
    }
}
