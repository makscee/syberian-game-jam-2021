using System;
using UnityEngine;

[ExecuteInEditMode]
public class Fish : MonoBehaviour
{
    public FishType type;
    public FishController controller;
    public FishTaskManager taskManager;
    public City city;
    public Food carriedFood;

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

    void Awake()
    {
        city = City.cities[(int) type];
        _position = transform.position;
        InitColor();
    }

    void InitColor()
    {
        var color = GlobalConfig.Instance.GetColorByType(type);
        foreach (var spriteRenderer in GetComponentsInChildren<SpriteRenderer>())
            spriteRenderer.color = color;
    }

    void OnValidate()
    {
        InitColor();
    }
}