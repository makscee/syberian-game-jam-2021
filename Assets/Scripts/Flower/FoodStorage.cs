using System;
using TMPro;
using UnityEngine;

[ExecuteInEditMode]
public class FoodStorage : MonoBehaviour
{
    public FishType type;
    public int resource;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] TextMeshProUGUI text;

    public Action onResourceChange;

    void OnValidate()
    {
        if (sr != null)
        {
            sr.color = GlobalConfig.Instance.GetColorByType(type).ChangeAlpha(sr.color.a);
        }

        if (text != null)
        {
            // text.color = GlobalConfig.Instance.paletteBg;
            text.color = Color.white;
            text.text = resource.ToString();
        }
    }

    public int Resource
    {
        get => resource;
        set
        {
            if (resource == value) return;
            resource = value;
            onResourceChange?.Invoke();
            if (text != null)
                text.text = value.ToString();
        }
    }
    
    public Food TakeFood(Fish actor)
    {
        if (Resource == 0)
            return null;
        Resource--;
        var food = Food.Create(type, actor.Position);
        food.AttachedTo = actor.gameObject;
        food.attachDistance = 0.1f;
        actor.carriedFood = food;
        return food;
    }

    public void PutFood(Fish actor, Food food)
    {
        Resource++;
        food.Destroy();
        actor.carriedFood = null;
    }
}