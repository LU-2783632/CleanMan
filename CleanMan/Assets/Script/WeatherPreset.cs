using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using RenderSettings = UnityEngine.RenderSettings;

[CreateAssetMenu(fileName = "WeatherPreset", menuName = "Weather System/Weather Preset")]
public class WeatherPreset : ScriptableObject
{
    [Header("Skybox Settings")]
    public Material skybox; // ��պв���


    [Header("Directional Light Settings")]
    public Color lightColor = Color.white; // ��Դ��ɫ
    public float lightIntensity = 1f; // ��Դǿ��

    [Header("Ambient Lighting Settings")]
    public Color ambientColor = Color.gray; // ��������ɫ
    public float ambientIntensity = 1f; // ������ǿ��

    [Header("Fog Settings")]
    public bool enableFog = false; // �Ƿ���������
    public Color fogColor = Color.gray; // ������ɫ
    public float fogDensity = 0.01f; // �����ܶ�

    [Header("Wind Settings")]
    public float windStrength = 1f; // ������С
    public float windRandomRange = 360f; // ���������Χ���ȣ�
    public float minWindInterval = 5f; // ����С���
    public float maxWindInterval = 10f; // �������
    public float windDuration = 2f; // �����ʱ��

    [Header("Audio Settings")]
    public AudioClip weatherSound; // ������Ч

    [Header("Rain Particle System Settings")]
    public bool enableRain = false; // �Ƿ�����������ϵͳ
    public GameObject rainParticlePrefab; // ������ϵͳԤ����

    //[Header("Falling Leaves Settings")]
    //public bool enableLeaves = false; // �Ƿ�������Ҷ
    //public float leafDuration = 5f;   // ÿ����Ҷ����ʱ��
    //public float minLeafInterval = 20f; // ��С��Ҷ���
    //public float maxLeafInterval = 30f; // �����Ҷ���
    //public int leafParticleCount = 10;  // ÿ����Ҷ����

}