using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// [ExecuteInEditMode]
public class City : MonoBehaviour, IPointerClickHandler, IPointerDownHandler
{
    public static City[] cities = new City[3];

    public FishType type;
    public float radius;
    public SpriteRenderer sr;
    public FoodStorage[] storages = new FoodStorage[3];
    public HashSet<Route> routes = new HashSet<Route>();

    void Awake()
    {
        cities[(int) type] = this;
        var flower = FindObjectOfType<Flower>();
        
    }

    public void AddFlowerRoute(Flower flower, int workers = 1)
    {
        var route = Route.Create(workers, new[] {flower.storage, storages[(int)flower.Type]});
        AddRoute(route);
    }

    public void AddCityRoute(City from, int workers = 1)
    {
        var route = Route.Create(workers, new[]
        {
            storages[(int) from.type],
            from.storages[(int) from.type],
            from.storages[(int) type],
            storages[(int) type]
        });
        AddRoute(route);
    }

    void AddRoute(Route route)
    {
        routes.Add(route);
        route.onDestroy += OnRouteDestroy;
    }

    void OnRouteDestroy(Route route)
    {
        routes.Remove(route);
    }

    public FishTask[] GetNewTask(Fish fish)
    {
        var fishRoute = GetFishRoute(fish);

        if (fishRoute != null)
        {
            if (fishRoute.WorkersAmount >= fishRoute.workers.Count)
            {
                return fishRoute.GetTasks(fish);
            }
            else
            {
                fishRoute.RemoveFish(fish);
                return new [] {new FishTask(fish, transform, FishTaskType.Move)};
            }
        }
        else
        {
            var vacantRoute = GetVacantRoute();
            if (vacantRoute != null)
            {
                vacantRoute.AssignFish(fish);
                return vacantRoute.GetTasks(fish);
            }
        }
        return new [] {new FishTask(fish, transform, FishTaskType.Move)};
    }

    Route GetFishRoute(Fish fish)
    {
        Route fishRoute = null;
        foreach (var route in routes)
        {
            if (route.workers.Contains(fish))
            {
                fishRoute = route;
                break;
            }
        }

        return fishRoute;
    }

    Route GetVacantRoute()
    {
        foreach (var route in routes)
            if (route.WorkersAmount > route.workers.Count)
                return route;
        return null;
    }

    void OnValidate()
    {
        transform.localScale = new Vector3(radius, radius, radius);
        sr.color = GlobalConfig.Instance.GetColorByType(type).ChangeAlpha(sr.color.a);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"click");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log($"clickkk");
    }
}