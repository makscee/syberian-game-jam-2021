using System;
using UnityEngine;

public class ArrowsMaterialPropertiesForwarder : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr;
    static readonly int Scale = Shader.PropertyToID("_Scale");
    static readonly int Color0 = Shader.PropertyToID("_Color0");

    void Update()
    {
        sr.material.SetFloat(Scale, transform.localScale.y * 6);
        sr.material.SetColor(Color0, sr.color);
    }
}