using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassengerGroupSelector : MonoBehaviour
{
    [Header("Passenger Groups (Index = Loop Number)")]
    public List<GameObject> loopPassengerGroups = new List<GameObject>();

    void Start()
    {
        // Listen to loop start event
        if (LoopCycleManager.instance != null)
        {
            LoopCycleManager.instance.OnLoopStarted += SetGroupForLoop;

            // Also set up immediately in case this is the first load
            SetGroupForLoop(LoopCycleManager.instance.loopCount);
        }
    }

    void SetGroupForLoop(int loop)
    {
        Debug.Log($"[Passengers] Setting group for loop {loop}");

        for (int i = 0; i < loopPassengerGroups.Count; i++)
        {
            loopPassengerGroups[i].SetActive(i == loop);
        }
    }

    void OnDestroy()
    {
        if (LoopCycleManager.instance != null)
            LoopCycleManager.instance.OnLoopStarted -= SetGroupForLoop;
    }
}
