using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public int screenWidth = Screen.width;
    [HideInInspector]
    public int screenHeight = Screen.height;

    static public GameManager instance { get { return Instance; } }
    static protected GameManager Instance;

    [HideInInspector]
    public int indexLevelToLoad;
    [HideInInspector]
    public bool levelCompleted;
    [HideInInspector]
    public Transform currentLevel;
    [HideInInspector]
    public bool gameIsPaused;

    public RoundPlayerController player;

    public Camera loadoutCamera;
    public Camera gameCamera;
    public Camera gameOverCamera;
    
    public SlowmotionController slowmotionController;

    public AState[] states;
    public AState topState { 
        get { 
            if (stateStack.Count == 0) 
                return null; 
            return stateStack[stateStack.Count - 1]; 
        } 
    }

    protected List<AState> stateStack = new List<AState>();
    protected Dictionary<string, AState> stateDict = new Dictionary<string, AState>();

    protected void OnEnable()
    {
        Instance = this;

        if (screenWidth == Screen.width || screenHeight == Screen.height)
        {
            screenHeight = Screen.width - (640 * 2);
            screenWidth = Screen.height - (360 * 2);
            Screen.SetResolution(screenWidth, screenHeight, true);
        }

        // We build a dictionnary from state for easy switching using their name.
        stateDict.Clear();

        if (states.Length == 0)
            return;

        for (int i = 0; i < states.Length; ++i)
        {
            states[i].manager = this;
            stateDict.Add(states[i].GetName(), states[i]);
        }

        stateStack.Clear();

        PushState(states[0].GetName());
    }

    protected void Update()
    {
        if (stateStack.Count > 0)
        {
            stateStack[stateStack.Count - 1].Tick();
        }
    }

    // State management
    public void SwitchState(string newState)
    {
        AState state = FindState(newState);
        if (state == null)
        {
            Debug.LogError("Can't find the state named " + newState);
            return;
        }

        stateStack[stateStack.Count - 1].Exit(state);
        state.Enter(stateStack[stateStack.Count - 1]);
        stateStack.RemoveAt(stateStack.Count - 1);
        stateStack.Add(state);
    }

    public AState FindState(string stateName)
    {
        AState state;
        if (!stateDict.TryGetValue(stateName, out state))
        {
            return null;
        }

        return state;
    }

    public void PopState()
    {
        if (stateStack.Count < 2)
        {
            Debug.LogError("Can't pop states, only one in stack.");
            return;
        }

        stateStack[stateStack.Count - 1].Exit(stateStack[stateStack.Count - 2]);
        stateStack[stateStack.Count - 2].Enter(stateStack[stateStack.Count - 2]);
        stateStack.RemoveAt(stateStack.Count - 1);
    }

    public void PushState(string name)
    {
        AState state;
        if (!stateDict.TryGetValue(name, out state))
        {
            Debug.LogError("Can't find the state named " + name);
            return;
        }

        if (stateStack.Count > 0)
        {
            stateStack[stateStack.Count - 1].Exit(state);
            state.Enter(stateStack[stateStack.Count - 1]);
        }
        else
        {
            state.Enter(null);
        }
        stateStack.Add(state);
    }
}

public abstract class AState : MonoBehaviour
{
    [HideInInspector]
    public GameManager manager;

    public abstract void Enter(AState from);
    public abstract void Exit(AState to);
    public abstract void Tick();

    public abstract string GetName();
}
