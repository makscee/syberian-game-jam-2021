using System;
using UnityEngine;

[ExecuteInEditMode]
public class Food : MonoBehaviour
{
    public FishType type;
    public SpriteRenderer sr;
    public Vector2 velocity;
    GameObject _attachedTo;
    bool _isAttached;

    public GameObject AttachedTo
    {
        get => _attachedTo;
        set
        {
            _attachedTo = value;
            _isAttached = value != null;
        }
    }

    void Update()
    {
        if (!_isAttached)
            return;
        var vec = _attachedTo.transform.position - transform.position;
        if (vec.magnitude > GlobalConfig.Instance.foodAttachDistance)
            velocity = vec.normalized;
        else velocity = Vector2.zero;
        transform.position += (Vector3)velocity * (Time.deltaTime * GlobalConfig.Instance.foodSpeed);
    }


    public void Destroy()
    {
        Destroy(gameObject);
    }
    public static Food Create(FishType type, Vector2 position)
    {
        var f = Instantiate(Prefabs.Instance.food, position, Quaternion.identity).GetComponent<Food>();
        f.type = type;
        f.sr.color = GlobalConfig.Instance.GetColorByType(type);
        return f;
    }
}