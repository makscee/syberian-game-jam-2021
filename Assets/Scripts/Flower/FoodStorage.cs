using System;
using TMPro;
using UnityEngine;

[ExecuteInEditMode]
public class FoodStorage : MonoBehaviour
{
    public const int MaxResource = 100;
    public FishType type;
    [SerializeField] int resource;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] bool textColorByType;

    public Action<int> onResourceChange;

    void OnValidate()
    {
        if (sr != null)
        {
            sr.color = GlobalConfig.Instance.GetColorByType(type).ChangeAlpha(sr.color.a);
        }

        if (text != null)
        {
            text.color = textColorByType ? GlobalConfig.Instance.GetColorByType(type) : GlobalConfig.Instance.paletteBg;
            text.text = resource.ToString();
        }
    }

    public bool IsFull => resource >= MaxResource;

    public int Resource
    {
        get => resource;
        set
        {
            value = Mathf.Min(MaxResource, value);
            if (resource == value) return;
            var delta = value - resource;
            resource = value;
            onResourceChange?.Invoke(delta);
            if (text != null)
                text.text = value.ToString();
        }
    }
    
    public bool TakeFood(Fish actor)
    {
        if (Resource == 0)
            return false;
        Resource--;
        var food = Food.Create(type, actor.Position);
        food.AttachedTo = actor.gameObject;
        actor.carriedFood = food;
        return true;
    }

    public void PutFood(Fish actor, Food food)
    {
        Resource++;
        food.Destroy();
        actor.carriedFood = null;
    }
}