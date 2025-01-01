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

    private float weatherTimer = 0f; // ������ʱ��
    private int weatherState = 0; // ��ǰ����״̬��0=���죬1=���죬2=����



    void Start()
    {

        // �� Resources �ļ��м�������Ԥ��
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

        if (weatherState == 0 && weatherTimer >= 60f) // �������60��
        {
            Debug.Log("Switching to Rainy Weather");
            weatherManager.StartWeatherTransition(rainyWeather);
            weatherState = 1;
            weatherTimer = 0f;
        }
        else if (weatherState == 1 && weatherTimer >= 40f) // �������40��
        {
            Debug.Log("Switching to Sunny Weather");
            weatherManager.StartWeatherTransition(sunnyWeather);
            weatherState = 2;
            weatherTimer = 0f;
        }
        else if (weatherState == 2 && weatherTimer >= 30f) // �������30��
        {
            Debug.Log("Switching to Cloudy Weather");
            weatherManager.StartWeatherTransition(cloudyWeather);
            weatherState = 0;
            weatherTimer = 0f;
        }
    }
    
}
