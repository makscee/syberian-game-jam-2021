using System;
using System.Globalization;
using TMPro;
using UnityEngine;

[ExecuteInEditMode]
public class Flower : MonoBehaviour
{
    public FishType Type => storage.type;
    public float radius;
    public FoodStorage storage;
    void OnValidate()
    {
        transform.localScale = new Vector3(radius, radius, radius);
    }
}