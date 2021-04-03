using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Flower.flowers[Random.Range(0, Flower.flowers.Count)].CreateSpore();
        }
    }
}