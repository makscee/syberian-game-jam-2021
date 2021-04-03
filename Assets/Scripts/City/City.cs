using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// [ExecuteInEditMode]
public class City : MonoBehaviour
{
    public static List<City> cities = new List<City>();

    public FishType type;
    public float radius;
    public SpriteRenderer sr;
    public FoodStorage[] storages = new FoodStorage[3];
    public HashSet<Route> routes = new HashSet<Route>();
    public HashSet<Fish> fishes = new HashSet<Fish>();

    FoodStorage OwnStorage => storages[(int) type];
    void Awake()
    {
        cities.Add(this);
    }

    void Start()
    {
        OwnStorage.onResourceChange += OnOwnFoodChange;
    }

    CityCreator _cityCreator;
    public void FishMoveIn(Fish f)
    {
        fishes.Add(f);
        f.onDeath += OnFishDeath;
        if (fishes.Count >= FoodStorage.MaxResource / 2 && _cityCreator == null)
        {
            _cityCreator = CityCreator.Create(this);
        }
    }

    void OnFishDeath(Fish f)
    {
        fishes.Remove(f);
        if (fishes.Count == 0)
            Destroy();
    }

    void OnOwnFoodChange(int delta)
    {
        if (delta > 0 && OwnStorage.Resource >= fishes.Count * 2)
        {
            OwnStorage.Resource -= fishes.Count;
            Fish.Create(transform.position, type);
        }
    }

    public void AddFlowerRoute(Flower flower, int workers = 1)
    {
        var storagesOrder = new[] {flower.storage, storages[(int) flower.Type]};
        foreach (var r in routes.Where(r => r.Compare(storagesOrder)))
        {
            r.WorkersAmount += workers;
            return;
        }

        var route = Route.Create(workers, storagesOrder);
        AddRoute(route);
        flower.onDestroy += f => route?.Destroy();
    }

    public void AddCityRoute(City from, int workers = 1)
    {
        var storagesOrder = new[]
        {
            storages[(int) from.type],
            from.storages[(int) from.type],
            from.storages[(int) type],
            storages[(int) type]
        };
        foreach (var r in routes.Where(r => r.Compare(storages)))
        {
            r.WorkersAmount += workers;
            return;
        }

        var route = Route.Create(workers, storagesOrder);
        AddRoute(route);
        from.onDestroy += c => route?.Destroy();
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
        if (fish.satiation < 0.5f && storages[(int) type].Resource > 0)
        {
            return new[]
                {new FishTask(fish, storages[(int) type].transform, FishTaskType.EatFood, storages[(int) type])};
        }
        
        var fishRoute = GetFishRoute(fish);

        if (_cityCreator != null && _cityCreator.neededFish > 0)
        {
            fishRoute?.RemoveFish(fish);
            var task = new FishTask(fish, _cityCreator.transform, FishTaskType.CreateCity);
            task.cityCreator = _cityCreator;
            return new[] {task};
        }

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
        Refresh();
    }

    void Refresh()
    {
        transform.localScale = new Vector3(radius, radius, radius);
        sr.color = GlobalConfig.Instance.GetColorByType(type).ChangeAlpha(sr.color.a);
    }

    public Action<City> onDestroy;
    public void Destroy()
    {
        foreach (var route in routes)
        {
            route.Destroy();
        }

        foreach (var fish in fishes)
        {
            fish.Die();
        }
        
        Destroy(gameObject);
        onDestroy?.Invoke(this);
    }

    public static City Create(Vector2 position, FishType type)
    {
        var c = Instantiate(Prefabs.Instance.city, position, Quaternion.identity).GetComponent<City>();
        c.type = type;
        c.Refresh();
        return c;
    }
}