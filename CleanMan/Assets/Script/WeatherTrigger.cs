using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WeatherTrigger : MonoBehaviour
{
    public WeatherManager weatherManager;
    public WeatherPreset sunnyWeather;
    public WeatherPreset cloudyWeather;
    public WeatherPreset rainyWeather;

    private float weatherTimer = 0f; // 天气计时器
    private int weatherState = 0; // 当前天气状态：0=阴天，1=雨天，2=晴天



    void Start()
    {

        // 从 Resources 文件夹加载天气预设
        sunnyWeather = Resources.Load<WeatherPreset>("WeatherPreset/SunnyDayWeatherPreset");
        cloudyWeather = Resources.Load<WeatherPreset>("WeatherPreset/CloudyWeatherPreset");
        rainyWeather = Resources.Load<WeatherPreset>("WeatherPreset/RainnyWeatherPreset");

        if (sunnyWeather == null || cloudyWeather == null || rainyWeather == null)
        {
            Debug.LogError("Failed to load WeatherPresets from Resources folder. Check your paths!");
        }

        if (weatherManager == null)
        {
            weatherManager = FindObjectOfType<WeatherManager>();
            if (weatherManager == null)
            {
                Debug.LogError("WeatherManager not found in the scene!");
            }
        }
        weatherManager.StartWeatherTransition(cloudyWeather);
    }

    void Update()
    {
        weatherTimer += Time.deltaTime;

        if (weatherState == 0 && weatherTimer >= 60f) // 阴天持续60秒
        {
            Debug.Log("Switching to Rainy Weather");
            weatherManager.StartWeatherTransition(rainyWeather);
            weatherState = 1;
            weatherTimer = 0f;
        }
        else if (weatherState == 1 && weatherTimer >= 40f) // 雨天持续40秒
        {
            Debug.Log("Switching to Sunny Weather");
            weatherManager.StartWeatherTransition(sunnyWeather);
            weatherState = 2;
            weatherTimer = 0f;
        }
        else if (weatherState == 2 && weatherTimer >= 30f) // 晴天持续30秒
        {
            Debug.Log("Switching to Cloudy Weather");
            weatherManager.StartWeatherTransition(cloudyWeather);
            weatherState = 0;
            weatherTimer = 0f;
        }
    }
    
}
