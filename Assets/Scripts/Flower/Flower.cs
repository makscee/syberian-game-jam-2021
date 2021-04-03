using System;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour
{
    public static List<Flower> flowers = new List<Flower>();
    const float MaxStorage = 100;
    public FishType Type => storage.type;
    public float radius, capacity;
    public FoodStorage storage;
    public Transform outerShellMask, resourceCircle, capacityCircle;

    void Start()
    {
        storage.onResourceChange += OnResourceChange;
        capacity = storage.Resource;
        RefreshRadius();
        RefreshColors();
        flowers.Add(this);
    }


    float _sinceLastTake, _regrowProgress, _sinceFullyGrown;
    void Update()
    {
        _sinceLastTake += Time.deltaTime;
        var shellPart = Mathf.Min(_sinceLastTake / GlobalConfig.Instance.flowerShellRegenDelay, 1f);
        outerShellMask.localScale = new Vector3(1.1f, shellPart * 1.1f);

        if (storage.Resource == MaxStorage)
            _sinceFullyGrown += Time.deltaTime;
        if (_sinceFullyGrown >= GlobalConfig.Instance.sporeCreateDelay) 
            CreateSpore();

        if (shellPart == 1f && capacity < MaxStorage)
        {
            capacity += Time.deltaTime * GlobalConfig.Instance.flowerShellRegenSpeed;
            RefreshRadius();
        }
        
        if (storage.Resource < Mathf.Floor(capacity))
        {
            _regrowProgress += GlobalConfig.Instance.flowerRegrowSpeed * Time.deltaTime;
            if (_regrowProgress >= 1f)
            {
                storage.Resource++;
                _regrowProgress--;
            }
        }
    }

    [SerializeField] bool _refresh;
    void OnValidate()
    {
        if (_refresh)
        {
            RefreshRadius();
            RefreshColors();
        }
    }

    void RefreshColors()
    {
        foreach (var spriteRenderer in GetComponentsInChildren<SpriteRenderer>())
        {
            spriteRenderer.color =
                GlobalConfig.Instance.GetColorByType(storage.type).ChangeAlpha(spriteRenderer.color.a);
            spriteRenderer.sprite = Prefabs.Instance.shapes[(int) storage.type];
        }
    }

    public void CreateSpore()
    {
        Spore.Create(this);
        storage.Resource = 0;
    }

    void OnResourceChange(int delta)
    {
        RefreshRadius();
        if (delta < 0)
        {
            _sinceLastTake = 0f;
            _sinceFullyGrown = 0f;
        }
    }

    void RefreshRadius()
    {
        radius = storage.Resource / MaxStorage;
        resourceCircle.localScale = new Vector3(radius, radius, radius);
        var capacityRadius = capacity / MaxStorage;
        capacityCircle.localScale = new Vector3(capacityRadius, capacityRadius, capacityRadius);
    }

    public Action<Flower> onDestroy;

    public void Destroy()
    {
        Destroy(gameObject);
        onDestroy?.Invoke(this);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        
    }

    public static Flower Create(Vector2 position, FishType type)
    {
        var flower = Instantiate(Prefabs.Instance.flower, position, Quaternion.identity).GetComponent<Flower>();
        flower.storage.type = type;
        flower.storage.Resource = 0;
        return flower;
    }
}