using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using RenderSettings = UnityEngine.RenderSettings;

[CreateAssetMenu(fileName = "WeatherPreset", menuName = "Weather System/Weather Preset")]
public class WeatherPreset : ScriptableObject
{
    [Header("Skybox Settings")]
    public Material skybox; // 天空盒材质


    [Header("Directional Light Settings")]
    public Color lightColor = Color.white; // 光源颜色
    public float lightIntensity = 1f; // 光源强度

    [Header("Ambient Lighting Settings")]
    public Color ambientColor = Color.gray; // 环境光颜色
    public float ambientIntensity = 1f; // 环境光强度

    [Header("Fog Settings")]
    public bool enableFog = false; // 是否启用雾气
    public Color fogColor = Color.gray; // 雾气颜色
    public float fogDensity = 0.01f; // 雾气密度

    [Header("Wind Settings")]
    public float windStrength = 1f; // 风力大小
    public float windRandomRange = 360f; // 风向随机范围（度）
    public float minWindInterval = 5f; // 风最小间隔
    public float maxWindInterval = 10f; // 风最大间隔
    public float windDuration = 2f; // 风持续时间

    [Header("Audio Settings")]
    public AudioClip weatherSound; // 天气音效

    [Header("Rain Particle System Settings")]
    public bool enableRain = false; // 是否启用雨粒子系统
    public GameObject rainParticlePrefab; // 雨粒子系统预制体

    //[Header("Falling Leaves Settings")]
    //public bool enableLeaves = false; // 是否启用落叶
    //public float leafDuration = 5f;   // 每次落叶持续时间
    //public float minLeafInterval = 20f; // 最小落叶间隔
    //public float maxLeafInterval = 30f; // 最大落叶间隔
    //public int leafParticleCount = 10;  // 每次落叶数量

}