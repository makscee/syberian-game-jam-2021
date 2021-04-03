using System.Collections.Generic;
using UnityEngine;

public class FishTaskManager : MonoBehaviour
{
    public Fish fish;

    public FishTask Current => GetCurrent(); 
    public List<FishTask> tasks = new List<FishTask>();

    FishTask GetCurrent()
    {
        if (tasks.Count > 0 && tasks[0].IsComplete())
            tasks.RemoveAt(0);
        if (tasks.Count == 0)
            tasks.AddRange(fish.city.GetNewTask(fish));
        return tasks[0];
    }

    public void ClearTasks()
    {
        tasks.Clear();
    }
}