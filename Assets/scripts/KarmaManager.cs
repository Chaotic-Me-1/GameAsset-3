using UnityEngine;

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
            DontDestroyOnLoad(gameObject); // Keep across scenes if needed
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddKarma(int amount)
    {
        karmaPoints = Mathf.Clamp(karmaPoints + amount, 0, 100);
        Debug.Log("Karma Changed: " + karmaPoints);
    }

    public void SubtractKarma(int amount)
    {
        karmaPoints = Mathf.Clamp(karmaPoints - amount, 0, 100);
        Debug.Log("Karma Changed: " + karmaPoints);
    }

    // Optional: For future scale visuals
    public float GetKarmaPercentage()
    {
        return karmaPoints / 100f; // Useful for animations later
    }
}

