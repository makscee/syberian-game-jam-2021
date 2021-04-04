using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spore : MonoBehaviour
{
    public Flower flower;
    Vector2 _flyDirection;
    [SerializeField] SpriteRenderer sr;
    void Start()
    {
        _flyDirection = Random.insideUnitSphere.normalized;
        sr.color = GlobalConfig.Instance.GetColorByType(flower.storage.type);
        _remainingDistance = GlobalConfig.Instance.sporeFlyMaxDistance;
    }

    float _remainingDistance;
    void Update()
    {
        var delta = (Vector3) _flyDirection * (GlobalConfig.Instance.sporeFlySpeed * Time.deltaTime);
        transform.position += delta;
        _remainingDistance -= delta.magnitude;
        if (_remainingDistance < 0f)
            Plant();
    }

    void Plant()
    {
        Flower.Create(transform.position, flower.Type, flower);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (flower != null && other.gameObject == flower.gameObject) return;
        _flyDirection = (transform.position - other.transform.position).normalized;
    }

    public static Spore Create(Flower f)
    {
        var spore = Instantiate(Prefabs.Instance.spore, f.transform.position, Quaternion.identity).GetComponent<Spore>();
        spore.flower = f;
        return spore;
    }
}