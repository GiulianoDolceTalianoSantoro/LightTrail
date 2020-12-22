using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Level
{
    public int index;
    public GameObject prefab;
}

/// <summary>
/// This is an asset which contains all the data for the game like the levels.
/// As an asset it live in the project folder, and get built into an asset bundle.
/// </summary>
[CreateAssetMenu(fileName = "gameData", menuName = "Light Trail/Game Data")]
public class GameData : ScriptableObject
{
    [Header("Objects")]
    public Level[] levels;
}
