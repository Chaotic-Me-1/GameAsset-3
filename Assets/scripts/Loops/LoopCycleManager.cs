using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class LoopCycleManager : MonoBehaviour
{
    public static LoopCycleManager instance;

    [Header("Current Loop Info")]
    public int loopCount = 0;

    public Action<int> OnLoopStarted; // Optional event to react to new loop in other scripts

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RestartScene()
    {
        loopCount++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Loop {loopCount}: Scene reloaded.");
        OnLoopStarted?.Invoke(loopCount);
        ApplyLoopChanges(loopCount);
    }

    private void ApplyLoopChanges(int loop)
    {
        // ✨ Put your conditional scene setup logic here
        // You can also split this logic into other scripts via OnLoopStarted
        switch (loop)
        {
            case 0:
                Debug.Log("First loop: Standard setup.");
                break;

            case 1:
                Debug.Log("Second loop: Change dialogue or remove props.");
                break;

            case 2:
                Debug.Log("Third loop: Reveal new memory or option.");
                break;

            default:
                Debug.Log("Later loops: Optional chaos...");
                break;
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
