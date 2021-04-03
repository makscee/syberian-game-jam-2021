using UnityEngine;

public class FishTask
{
    const float ReachedDistanceThreshold = 0.1f;
    
    public Transform targetObject;
    public Vector2 target => (Vector2) targetObject.position + inRadiusOffset * targetObject.localScale.x / 2;
    Vector2 inRadiusOffset;
    public FishTaskType type;
    public Fish actor;
    public FoodStorage storage;
    public CityCreator cityCreator;

    public FishTask(Fish actor, Transform targetObject, FishTaskType type, FoodStorage storage = null)
    {
        this.actor = actor;
        this.targetObject = targetObject;
        this.storage = storage;
        this.type = type;
        inRadiusOffset = Random.onUnitSphere;
    }

    bool _complete;
    public bool IsComplete()
    {
        if (type == FishTaskType.Move)
        {
            return IsReached();
        }
        else return _complete;
    }

    public void SetComplete()
    {
        _complete = true;
    }

    public bool IsReached()
    {
        return targetObject == null || ((Vector2)actor.transform.position - target).magnitude < ReachedDistanceThreshold;
    }
}

public enum FishTaskType
{
    Move, TakeFood, DepositFood, EatFood, CreateCity
}