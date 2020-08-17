using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Camera mainCamera;
    public Camera goalCamera;

    private int cameraIndex;

    bool levelCompleted;

    // Start is called before the first frame update
    void Start()
    {
        cameraIndex = 0;

        mainCamera.enabled = true;
        mainCamera.GetComponent<AudioListener>().enabled = true;

        goalCamera.enabled = false;
        goalCamera.GetComponent<AudioListener>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        IsLevelComplete();
    }

    void IsLevelComplete()
    {
        if (RoundPlayerController.goalReached && cameraIndex == 0)
        {
            mainCamera.enabled = false;
            mainCamera.GetComponent<AudioListener>().enabled = false;

            goalCamera.enabled = true;
            goalCamera.GetComponent<AudioListener>().enabled = true;

            cameraIndex = 1;
            levelCompleted = true;
        }
    }

    void ChangeScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
