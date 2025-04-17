using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNameManager : MonoBehaviour
{
    public static PlayerNameManager instance;
    public string playerName;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // prevent duplicates
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // survive scene loads
    }

    public void SetPlayerName(string name)
    {
        playerName = name;
        Debug.Log("Player name set to: " + name);
    }

    public string GetPlayerName()
    {
        return playerName;
    }
}