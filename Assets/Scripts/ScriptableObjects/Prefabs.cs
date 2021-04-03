using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Prefabs", menuName = "ScriptableObjects/Prefabs")]
public class Prefabs : ScriptableObject
{
    public GameObject food, routeObject, spore, flower, fish, city, cityCreator;

    public static Prefabs Instance => GetInstance();

    static Prefabs _instanceCache;
    static Prefabs GetInstance()
    {
        if (_instanceCache == null)
            _instanceCache = Resources.Load<Prefabs>("Prefabs");
        return _instanceCache;
    }
}