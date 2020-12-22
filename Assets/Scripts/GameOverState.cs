using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverState : AState
{
    public GameObject gameOverCanvas;

    private RoundPlayerController player;

    public RectTransform gameOverPopup;
    public GameObject timeText;

    private GameObject winningGround;

    public override void Enter(AState from)
    {
        manager.gameOverCamera.gameObject.SetActive(true);

        gameOverCanvas.gameObject.SetActive(true);

        winningGround = GameObject.FindGameObjectWithTag("WinningGround");
        player = FindObjectOfType<RoundPlayerController>();
        timeText.GetComponent<TextMeshProUGUI>().text = string.Format("{0} s.", player.time);

        manager.gameOverCamera.transform.position = new Vector3(player.transform.position.x + 50f, player.transform.position.y, player.transform.position.z + 100f);

        StartCoroutine(WaitForGameOverPopup());
    }

    public override void Exit(AState to)
    {
        // Free resources (environment and enemies)
        gameOverPopup.gameObject.SetActive(false);
        gameOverCanvas.gameObject.SetActive(false);
        Destroy(winningGround.gameObject);
    }

    public override string GetName()
    {
        return "Game Over";
    }

    IEnumerator WaitForGameOverPopup()
    {
        manager.levelCompleted = true;

        yield return new WaitForSeconds(1f);
        OpenGameOverPopup();
    }

    public void OpenGameOverPopup()
    {
        gameOverPopup.gameObject.SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        manager.SwitchState("Loadout");
    }

    public void Retry()
    {
        player.isRetry = true;
        manager.SwitchState("Game");
    }

    public override void Tick()
    {
        manager.gameOverCamera.transform.LookAt(player.transform);
    }
}
