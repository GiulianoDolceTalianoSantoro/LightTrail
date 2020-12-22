using System.Collections.Generic;
using System.ComponentModel;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class GameState : AState
{
    public Canvas gameStateCanvas;
    public Canvas gameOverCanvas;

    private RoundPlayerController player;

    public GameData currentGameData;

    public GameObject cameraHolder;
    private GameObject level;
    private Transform levelHolder;

    private float timeSinceStart;

    private bool isFinished;

    [Header("UI")]
    public GameObject timeText;

    public RectTransform UI;
    public RectTransform pauseMenu;

    private GameObject goal;
    public GameObject goalPrefab;

    public GameObject enemyPrefab;

    public override void Enter(AState from)
    {
        if (player != null && player.isRetry)
        {
            OnRetry();
        }
        else
        {
            InstantiatePlayer();
        }

        manager.gameCamera.gameObject.SetActive(true);
        manager.gameCamera.GetComponent<CameraController>().target = player.gameObject;

        gameStateCanvas.gameObject.SetActive(true);
        UI.gameObject.SetActive(true);
        player.currentLife = 3;
        player.goalReached = false;

        levelHolder = GameObject.FindGameObjectWithTag("LevelHolder").transform;

        level = levelHolder.Find($"Level_{manager.indexLevelToLoad}").gameObject;

        try
        {
            InitLevel(manager.indexLevelToLoad);
        }
        catch (Exception ex)
        {
            Debug.LogError(string.Format("No level with that index bro ", ex));
        }
    }

    public override void Exit(AState to)
    {
        gameStateCanvas.gameObject.SetActive(false);
        manager.gameCamera.gameObject.SetActive(false);

        level.gameObject.SetActive(false);

        FreeResources();
    }

    private void FreeResources()
    {
        var enemiesParent = manager.currentLevel.Find("Enemies");

        for (int i = 0; i < enemiesParent.childCount; i++)
        {
            var enemy = enemiesParent.GetChild(i);
            Destroy(enemy.gameObject);
        }

        var bullets = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (var bullet in bullets)
        {
            Destroy(bullet.gameObject);
        }

        Destroy(goal.gameObject);
    }

    public void InitLevel(int levelIndex)
    {
        gameStateCanvas.gameObject.SetActive(true);
        UI.gameObject.SetActive(true);
        pauseMenu.gameObject.SetActive(false);

        var levelName = !player.isRetry ? $"Level_{levelIndex}" : manager.currentLevel.name;

        var level = levelHolder.Find(levelName);
        manager.currentLevel = level;
        level.transform.position = new Vector3(0f, 0f, 0f);
        level.gameObject.SetActive(true);

        InstantiateGoal();
        InstantiateEnemies();

        isFinished = false;
        manager.gameIsPaused = false;
        manager.levelCompleted = false;
        timeSinceStart = 0;
    }

    void InstantiatePlayer()
    {
        player = Instantiate(manager.player);
    }

    void InstantiateGoal()
    {
        var goalPoint = manager.currentLevel.GetComponent<LevelStructure>().goalPoint;
        if (goalPoint != null)
        {
            Transform point = goalPoint.transform;
            goal = Instantiate(goalPrefab, point.position, Quaternion.identity);
        }
    }

    void InstantiateEnemies()
    {
        var enemyPositions = manager.currentLevel.GetComponent<LevelStructure>().enemyPositionsParent;
        if (enemyPositions != null)
        {
            for (int i = 0; i < enemyPositions.childCount; i++)
            {
                Transform enemy = enemyPositions.GetChild(i);
                var prefab = Instantiate(enemyPrefab, enemy.position, Quaternion.identity);
                prefab.transform.SetParent(manager.currentLevel.Find("Enemies"), true);
            }
        }
    }

    void OnRetry()
    {
        player = FindObjectOfType<RoundPlayerController>();
        Destroy(player.gameObject);
        player = Instantiate(manager.player);

        manager.slowmotionController.slowdownLength = 0.5f;

        ResetTrail();
    }

    void ResetTrail()
    {
        player.transform.GetComponent<TrailRenderer>().Clear();
    }

    public override string GetName()
    {
        return "Game";
    }

    public void GameOver()
    {
        manager.SwitchState("Game Over");
    }

    float timeUntilPause;
    Vector3 velocityUntilPause;
    public void Pause()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !manager.gameIsPaused)
        {
            manager.gameIsPaused = true;

            timeUntilPause = float.Parse(timeText.GetComponent<TextMeshProUGUI>().text);
            timeText.gameObject.SetActive(false);

            //check if we aren't finished OR if we aren't already in pause (as that would mess states)
            if (isFinished || AudioListener.pause == true)
                return;

            Time.timeScale = 0;

            var bullets = GameObject.FindGameObjectsWithTag("Bullet");
            foreach (var bullet in bullets)
            {
                bullet.GetComponent<BulletController>().OnPause();
            }

            velocityUntilPause = player.rb.velocity;
            player.rb.velocity = Vector3.zero;
            pauseMenu.gameObject.SetActive(true);
            UI.gameObject.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && manager.gameIsPaused)
        {
            Resume(timeUntilPause);
        }
    }

    public void Resume(float timeUntilPaused)
    {
        timeSinceStart = timeUntilPaused;
        Time.timeScale = 1.0f;
        player.rb.velocity = velocityUntilPause;

        manager.gameIsPaused = false;

        var bullets = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (var bullet in bullets)
        {
            bullet.GetComponent<BulletController>().OnResume();
        }

        pauseMenu.gameObject.SetActive(false);
        UI.gameObject.SetActive(true);

        AudioListener.pause = false;
    }

    public void QuitToLoadout()
    {
        // Used by the pause menu to return immediately to loadout, canceling everything.
        AudioListener.pause = false;

        manager.SwitchState("Loadout");
    }

    public override void Tick()
    {
        timeSinceStart += Time.unscaledDeltaTime;

        UpdateUI();

        Pause();

        if (player.goalReached /*|| player.currentLife <= 0*/)
        {

            player.time = timeText.GetComponent<TextMeshProUGUI>().text;

            GameOver();
        }
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    protected void UpdateUI()
    {
        timeText.GetComponent<TextMeshProUGUI>().text = timeSinceStart.ToString("F0");
    }
}
