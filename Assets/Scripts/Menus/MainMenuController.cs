using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public List<CustomButton> customButtons;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StoryModeButton();
        MultiplayerButton();
        OptionsButton();
    }

    void StoryModeButton()
    {
        var storyModeButton = customButtons.Find(i => i.name == "StoryModeButton");
        if (storyModeButton.isClicked)
        {
            Debug.Log($"{storyModeButton.name} clicked!");
            SceneManager.LoadScene("Level_1");
        }
    }

    void MultiplayerButton()
    {
        var multiplayerButton = customButtons.Find(i => i.name == "MultiplayerButton");
        if (multiplayerButton.isClicked)
        {
            Debug.Log($"{multiplayerButton.name} clicked!");
        }
    }

    void OptionsButton()
    {
        var optionsButton = customButtons.Find(i => i.name == "OptionsButton");
        if (optionsButton.isClicked)
        {
            Debug.Log($"{optionsButton.name} clicked!");
        }
    }
}
