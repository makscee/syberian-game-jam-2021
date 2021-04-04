using System;
using System.Linq;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public FishType type;
    public FishController controller;
    public FishTaskManager taskManager;
    public Transform satiationCircle;
    public City city;
    public Food carriedFood;
    public float satiation;

    Vector2 _position;
    public Vector2 Position
    {
        get => _position;
        set
        {
            transform.position = value;
            _position = value;
        }
    }

    public void EatFood()
    {
        if (carriedFood == null) return;
        satiation += 0.5f;
        carriedFood.Destroy();
    }

    void Start()
    {
        city = City.cities.Where(c => c.type == type).Aggregate((curMin, c) =>
            curMin == null || CityDistance(c) < CityDistance(curMin) ? c : curMin);
        city.FishMoveIn(this);
        _position = transform.position;
        satiation = 1;
        InitColor();
    }

    void Update()
    {
        satiation -= GlobalConfig.Instance.satiationDepletionRate * Time.deltaTime;
        SatiationCircleRefresh();
        if (satiation < 0)
        {
            Die();
        }
    }

    public Action<Fish> onDeath;
    public void Die()
    {
        if (carriedFood != null) carriedFood.Destroy();
        Destroy(gameObject);
        onDeath?.Invoke(this);
    }

    void SatiationCircleRefresh()
    {
        satiationCircle.localScale = new Vector3(satiation, satiation);
    }

    float CityDistance(City c)
    {
        return ((Vector2) c.transform.position - _position).magnitude;
    }

    void InitColor()
    {
        var c = GlobalConfig.Instance.GetColorByType(type);
        foreach (var spriteRenderer in GetComponentsInChildren<SpriteRenderer>())
        {
            spriteRenderer.color = c.ChangeAlpha(spriteRenderer.color.a);
            spriteRenderer.sprite = Prefabs.Instance.shapes[(int) type];
        }
    }

    void OnValidate()
    {
        InitColor();
    }

    public static Fish Create(Vector2 position, FishType type)
    {
        var f = Instantiate(Prefabs.Instance.fish, position, Quaternion.identity).GetComponent<Fish>();
        f.type = type;
        f.Position = position;
        return f;
    }
}