using UnityEngine;
using System.Collections;

public class WindZoneRandomDirection : MonoBehaviour {

    public WeatherManager weatherManager; // 引用 WeatherManager
    public Vector2 groundAreaSize = new Vector2(15f, 15f); // 风范围（地面区域大小）
    public Transform groundCenter; // 地面中心点

    public AudioSource windAudioSource; // 风声音源
    public AnimationCurve volumeCurve; // 音量曲线，根据风力调整

    private float timer = 0f; // 计时器
    private float nextWindTime = 5f; // 下一次风间隔时间
    private bool isWindActive = false;

    void Start()
    {
        if (windAudioSource == null)
        {
            Debug.LogError("WindAudioSource is not assigned!");
        }

        // 初始化音量
        if (windAudioSource != null)
        {
            windAudioSource.volume = 0f;
            windAudioSource.loop = true;
            windAudioSource.Play();
        }
    }

    void Update ()
    {
        WeatherPreset currentWeather = weatherManager.GetCurrentWeather();
        timer += Time.deltaTime;

        if (isWindActive && timer >= currentWeather.windDuration)
        {
            // 停止风
            StopWind();
        }

        if (!isWindActive && timer >= nextWindTime)
        {
            // 激活风
            StartWind(currentWeather);
        }

        // 动态调整音量
        if (isWindActive && windAudioSource != null)
        {
            AdjustWindSound();
        }
    }

    private void StartWind(WeatherPreset currentWeather)
    {
        nextWindTime = Random.Range(currentWeather.minWindInterval, currentWeather.maxWindInterval);
        timer = 0f;

        // 随机设置风的位置
        Vector3 randomPosition = GetRandomPositionWithinGround();
        transform.position = randomPosition;

        // 随机设置风的方向
        WindZone windZone = GetComponent<WindZone>();
        Vector3 randomDirection = Quaternion.Euler(0, Random.Range(0, 360), 0) * Vector3.forward;
        windZone.windMain = currentWeather.windStrength;
        windZone.windTurbulence = 1f;
        windZone.windPulseMagnitude = 0.5f;
        windZone.windPulseFrequency = 0.5f;
        transform.rotation = Quaternion.LookRotation(randomDirection);

        isWindActive = true;
    }

    private void StopWind()
    {
        // 停止风
        WindZone windZone = GetComponent<WindZone>();
        windZone.windMain = 0f;
        windZone.windTurbulence = 0f;
        windZone.windPulseMagnitude = 0f;
        windZone.windPulseFrequency = 0f;

        // 停止风声
        if (windAudioSource != null)
        {
            windAudioSource.volume = 0f; // 确保音量为零
        }

        isWindActive = false;
        timer = 0f;
    }

    private void AdjustWindSound()
    {
        WindZone windZone = GetComponent<WindZone>();
        float targetVolume = volumeCurve.Evaluate(windZone.windMain); // 根据风力计算音量
        windAudioSource.volume = Mathf.Lerp(windAudioSource.volume, targetVolume, Time.deltaTime * 2f); // 平滑过渡音量
    }


    private Vector3 GetRandomPositionWithinGround()
    {
        // 在地面范围内生成随机位置
        if (groundCenter == null) return Vector3.zero;

        float randomX = Random.Range(-groundAreaSize.x / 2f, groundAreaSize.x / 2f);
        float randomZ = Random.Range(-groundAreaSize.y / 2f, groundAreaSize.y / 2f);
        return groundCenter.position + new Vector3(randomX, 27f, randomZ);
    }
}
