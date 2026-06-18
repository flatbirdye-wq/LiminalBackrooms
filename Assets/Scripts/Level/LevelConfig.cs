using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Backrooms/LevelConfig")]
public class LevelConfig : ScriptableObject
{
    public string id;
    public string displayName;
    public string alias;
    [TextArea(3,10)]
    public string environmentDescription;
    public string difficulty;
    public float entityMultiplier = 1.0f;
    public int defaultSeed = 0;

    [Header("Generation parameters")]
    public int roomMinSize = 6;
    public int roomMaxSize = 20;
    public float deadEndChance = 0.25f;
    public float anomalyRarity = 0.01f;
    public bool darknessOverridesLights = false;
}
