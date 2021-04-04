using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Update()
    {
        Animator.Update();
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