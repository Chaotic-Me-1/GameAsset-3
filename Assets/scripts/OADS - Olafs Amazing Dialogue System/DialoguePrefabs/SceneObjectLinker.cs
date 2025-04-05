using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObjectLinker : MonoBehaviour
{
    public static SceneObjectLinker instance;

    [System.Serializable]
    public class ObjectEntry
    {
        public string ID;
        public GameObject targetObject;
    }

    public List<ObjectEntry> objectEntries = new List<ObjectEntry>();

    private Dictionary<string, GameObject> objectLookup;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        objectLookup = new Dictionary<string, GameObject>();
        foreach (var entry in objectEntries)
        {
            if (!objectLookup.ContainsKey(entry.ID))
                objectLookup.Add(entry.ID, entry.targetObject);
        }
    }

    public GameObject GetObjectByID(string id)
    {
        if (objectLookup != null && objectLookup.TryGetValue(id, out var obj))
            return obj;
        return null;
    }
}
