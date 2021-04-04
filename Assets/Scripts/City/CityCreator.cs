using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class CityCreator : MonoBehaviour
{
    public City city;
    Vector2 _flyDirection;
    [SerializeField] SpriteRenderer sr;
    public int neededFish = 25;
    void Start()
    {
        _flyDirection = Random.insideUnitSphere.normalized;
        transform.position += (Vector3) _flyDirection * 3;
        sr.color = GlobalConfig.Instance.GetColorByType(city.type);
        _remainingDistance = GlobalConfig.Instance.cityCreatorMaxDistance;
    }

    float _remainingDistance;
    void Update()
    {
        if (neededFish > 0) return;
        var delta = (Vector3) _flyDirection * (GlobalConfig.Instance.cityCreatorSpeed * Time.deltaTime);
        transform.position += delta;
        _remainingDistance -= delta.magnitude;
        if (_remainingDistance < 0f)
            CreateCity();
    }

    public Action whenDone;
    void CreateCity()
    {
        var pos = transform.position;
        var c = City.Create(pos, city.type);
        // foreach (var flower in Flower.flowers.Where(flower => (flower.transform.position - pos).magnitude < 2))
        //     flower.Destroy();
        Fish.Create(pos, c.type);
        Destroy(gameObject);
        whenDone?.Invoke();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Route"))
            return;
        _flyDirection = (transform.position - other.transform.position).normalized;
    }

    public void AddFish(Fish f)
    {
        if (neededFish == 0) return; 
        neededFish--;
        f.Die();
    }

    public static CityCreator Create(City c)
    {
        var cc = Instantiate(Prefabs.Instance.cityCreator, c.transform.position, Quaternion.identity).GetComponent<CityCreator>();
        cc.city = c;
        return cc;
    }
}