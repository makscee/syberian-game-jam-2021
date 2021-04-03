using System;
using UnityEngine;

public class MouseDragObject : MonoBehaviour
{
    public static MouseDragObject instance;

    void Awake()
    {
        instance = this;
        SetActive(false);
    }

    public Vector2 from, to;

    void Update()
    {
        var vec = to - from;
        var angle = Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg - 90;
        transform.position = from + vec / 2;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.localScale = new Vector3(.05f, vec.magnitude, 1);
    }

    public void SetActive(bool value)
    {
        gameObject.SetActive(value);
    }
}