using System;
using System.Collections.Generic;
using UnityEngine;

public class Route
{
    int workersAmount;
    public HashSet<Fish> workers = new HashSet<Fish>();
    public FoodStorage[] storagesOrder; // take from 0 put to 1, take from 2 put to 3 ...

    public Action onWorkersAmountChange;
    public int WorkersAmount
    {
        get => workersAmount;
        set
        {
            workersAmount = value;
            onWorkersAmountChange?.Invoke();
            if (workersAmount <= 0)
            {
                Destroy();
            }
        }
    }

    Route(int workers, FoodStorage[] storages)
    {
        workersAmount = workers;
        storagesOrder = storages;
    }

    public bool Compare(FoodStorage[] storages)
    {
        return storages[0] == storagesOrder[0] && storages[1] == storagesOrder[1];
    }

    public static Route Create(int workersAmount, FoodStorage[] storagesOrder)
    {
        var r = new Route(workersAmount, storagesOrder);
        RouteObject.Create(r);
        return r;
    }

    public Action<Route> onDestroy;
    public void Destroy()
    {
        onDestroy?.Invoke(this);
    }

    public static void TryCreateFromTwoObjects(GameObject from, GameObject to)
    {
        Flower flower = null;
        City city = null;
        if (from.CompareTag("Flower") && to.CompareTag("City"))
        {
            flower = from.GetComponent<Flower>();
            city = to.GetComponent<City>();
        } else if (to.CompareTag("Flower") && from.CompareTag("City"))
        {
            flower = to.GetComponent<Flower>();
            city = from.GetComponent<City>();
        }

        if (flower != null && city != null)
        {
            city.AddFlowerRoute(flower);
            return;
        }

        if (from.CompareTag("City") && to.CompareTag("City"))
        {
            var cityFrom = from.GetComponent<City>();
            var cityTo = to.GetComponent<City>();
            cityFrom.AddCityRoute(cityTo);
        }
    }

    public FishTask[] GetTasks(Fish fish)
    {
        var tasks = new FishTask[storagesOrder.Length];
        var ind = 0;
        foreach (var foodStorage in storagesOrder)
        {
            if (foodStorage == null) return null;
            tasks[ind] = new FishTask(fish, foodStorage.transform,
                ind % 2 == 0 ? FishTaskType.TakeFood : FishTaskType.DepositFood, foodStorage);
            ind++;
        }

        return tasks;
    }

    public void AssignFish(Fish fish)
    {
        workers.Add(fish);
    }

    public void RemoveFish(Fish fish)
    {
        workers.Remove(fish);
    }
}