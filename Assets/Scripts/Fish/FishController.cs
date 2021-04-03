using System;
using UnityEngine;

public class FishController : MonoBehaviour
{
    public Fish fish;
    void Update()
    {
        var task = fish.taskManager.Current;
        if (!task.IsReached())
            fish.Position += (task.target - fish.Position).normalized * (GlobalConfig.Instance.fishSpeed * Time.deltaTime);
        else if (!task.IsComplete())
        {
            DoTaskAction(task);
            task.SetComplete();
        }
    }

    void DoTaskAction(FishTask task)
    {
        switch (task.type)
        {
            case FishTaskType.Move:
                break;
            case FishTaskType.TakeFood:
                if (task.storage != null && task.storage.TakeFood(fish))
                    break;
                fish.taskManager.ClearTasks();
                break;
            case FishTaskType.DepositFood:
                if (fish.carriedFood != null && task.storage != null)
                    task.storage.PutFood(fish, fish.carriedFood);
                break;
            case FishTaskType.EatFood:
                if (task.storage != null)
                    task.storage.TakeFood(fish);
                fish.EatFood();
                break;
            case FishTaskType.CreateCity:
                task.cityCreator.AddFish(fish);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}