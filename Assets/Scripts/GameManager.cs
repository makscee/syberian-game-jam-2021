using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    void Update()
    {
        Animator.Update();
        if (Input.GetKeyDown(KeyCode.S))
        {
            Flower.flowers[Random.Range(0, Flower.flowers.Count)].CreateSpore();
        }
    }

    void LateUpdate()
    {
        timeScaleUpdatedThisFrame = false;
    }

    public static bool timeScaleUpdatedThisFrame;
    public void UpdateSpeedScale(Single value)
    {
        Time.timeScale = Mathf.Lerp(0.3f, 15f, value);
        timeScaleUpdatedThisFrame = true;
    }
}