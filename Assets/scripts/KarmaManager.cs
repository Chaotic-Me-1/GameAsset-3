using UnityEngine;

// Script by OlafRT
// This keeps track of the karma points the player has,
// starting at a neutral 50.

public class KarmaManager : MonoBehaviour
{
    public static KarmaManager instance;

    [Range(0, 100)]
    public int karmaPoints = 50; // Start neutral

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Makes sure we still have the current amount of karma across the loops.
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddKarma(int amount)
    {
        karmaPoints = Mathf.Clamp(karmaPoints + amount, 0, 100);
        Debug.Log("Karma changed: " + karmaPoints);
    }

    public void SubtractKarma(int amount)
    {
        karmaPoints = Mathf.Clamp(karmaPoints - amount, 0, 100);
        Debug.Log("Karma changed: " + karmaPoints);
    }

    public float GetKarmaPercentage()
    {
        return karmaPoints / 100f;
    }
}

