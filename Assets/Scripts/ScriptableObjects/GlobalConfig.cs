using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GlobalConfig", menuName = "ScriptableObjects/GlobalConfig")]
public class GlobalConfig : ScriptableObject
{
    public Color paletteCircle, paletteTriangle, paletteSquare, paletteBg;
    public float fishSpeed, satiationDepletionRate;
    public float foodAttachDistance, foodSpeed;

    public float flowerRegrowSpeed, flowerShellRegenDelay, flowerShellRegenSpeed, sporeCreateDelay;
    public float sporeFlyMaxDistance, sporeFlySpeed;

    public float cityCreatorSpeed, cityCreatorMaxDistance;

    public static GlobalConfig Instance => GetInstance();

    static GlobalConfig _instanceCache;

    static GlobalConfig GetInstance()
    {
        if (_instanceCache == null)
            _instanceCache = Resources.Load<GlobalConfig>("GlobalConfig");
        return _instanceCache;
    }

    public Color GetColorByType(FishType type)
    {
        switch (type)
        {
            case FishType.Circle:
                return paletteCircle;
            case FishType.Square:
                return paletteSquare;
            case FishType.Triangle:
                return paletteTriangle;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
}