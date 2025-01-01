using System.Collections;
using System.Collections.Generic;
using UnityEditor.Presets;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    public Light directionalLight;           // ����Դ
    public WindZone windZone;                // ��������
    //public LeafSpawnerWithPool leafSpawner; // ��Ҷ������
    public RainSpawnerWithPool rainSpawner; // ��ι�����

    private WeatherPreset currentWeather;    // ��ǰ����
    private WeatherPreset targetWeather;     // Ŀ������
    private float transitionProgress = 0f;  // ���ɽ���
    public float transitionDuration = 5f;   // ����ʱ��
    private bool isTransitioning = false; // ����Ƿ����ڹ���

    public WeatherPreset cloudyWeather;
    public AudioSource audioSource; // ��Ч������


    void Start()
    {
        if (cloudyWeather == null)
        {
            Debug.LogError("CloudyWeather preset is not assigned in WeatherManager!");
            return;
        }


        if (currentWeather == null)
        {
            currentWeather = cloudyWeather;
            ApplyWeather(currentWeather);
            Debug.Log($"Initial currentWeather set to: {currentWeather.name}");
        }
        else
        {
            Debug.Log($"currentWeather was already initialized to: {currentWeather.name}");
        }
    }

    public void StartWeatherTransition(WeatherPreset newWeather)
    {
        if (isTransitioning)
        {
            Debug.Log($"Weather transition in progress. Skipping transition to {newWeather.name}");
            return;
        }

        if (newWeather == null)
        {
            Debug.LogError("Target WeatherPreset is null!");
            return;
        }

        Debug.Log($"Starting transition to weather preset: {newWeather.name}");
        targetWeather = newWeather;
        transitionProgress = 0f; // ���ù��ɽ���
        StopAllCoroutines();
        StartCoroutine(TransitionWeather());
    }
    public WeatherPreset GetCurrentWeather()
    {
        return currentWeather;
    }

    private IEnumerator TransitionWeather()
    {
        
        //currentWeather = targetWeather;

        if (currentWeather == null)
        {
            Debug.LogError("Initial WeatherPreset (currentWeather) is null in TransitionWeather! Ensure it is properly initialized.");
            yield break;
        }


        //if (targetWeather == null)
       // {
            //Debug.LogError("Target WeatherPreset is null in TransitionWeather! Ensure StartWeatherTransition is called with a valid preset.");
            //yield break;
       // }

        Debug.Log($"Transitioning from {currentWeather.name} to {targetWeather.name}...");

        isTransitioning = true;
        // �����ʼ״̬
        WeatherPreset initialWeather = currentWeather;

        while (transitionProgress < 1f)
        {
            transitionProgress += Time.deltaTime / transitionDuration;
            ApplyWeatherLerp(initialWeather, targetWeather, transitionProgress);
            yield return null;
        }

        while (transitionProgress < 1f)
        {
            transitionProgress += Time.deltaTime / transitionDuration;

            // ��������
            ApplyWeatherLerp(initialWeather, targetWeather, transitionProgress);

            // ��̬���������Ӻ� Ripple ǿ��
            if (rainSpawner)
            {
                float targetRainIntensity = targetWeather.enableRain ? 1f : 0f;
                rainSpawner.SetRainIntensity(targetRainIntensity * transitionProgress);
            }

            yield return null; // �ȴ���һ֡
        }

        // ����Ӧ��Ŀ������
        ApplyWeather(targetWeather);
        currentWeather = targetWeather;
        isTransitioning = false; // �������

        Debug.Log($"Weather transition completed. CurrentWeather is now {currentWeather.name}.");
    }

    private void ApplyWeatherLerp(WeatherPreset from, WeatherPreset to, float t)
    {
        if (from == null )
        {
            Debug.LogError("From WeatherPreset is null in ApplyWeatherLerp!");
            return;
        }

        if (to == null)
        {
            Debug.LogError("To WeatherPreset is null in ApplyWeatherLerp!");
            return;
        }

       // Debug.Log($"Interpolating from {from.name} to {to.name} at progress {t}.");

        // ��պн���
        if (from.skybox != null && to.skybox != null)
        {
            RenderSettings.skybox.Lerp(from.skybox, to.skybox, t);
        }
        else if (from.skybox != null)
        {
            RenderSettings.skybox = from.skybox; // ��Ӧ����Դ skybox
        }
        else if (to.skybox != null)
        {
            RenderSettings.skybox = to.skybox; // ��Ӧ��Ŀ�� skybox
        }

        // ��Դ����
        if (directionalLight)
        {
            directionalLight.color = Color.Lerp(from.lightColor, to.lightColor, t);
            directionalLight.intensity = Mathf.Lerp(from.lightIntensity, to.lightIntensity, t);
        }

        // �����⽥��
        RenderSettings.ambientLight = Color.Lerp(from.ambientColor, to.ambientColor, t);
        RenderSettings.ambientIntensity = Mathf.Lerp(from.ambientIntensity, to.ambientIntensity, t);

        // ��������
        if (from.enableFog || to.enableFog)
        {
            RenderSettings.fog = true;
            RenderSettings.fogColor = Color.Lerp(from.fogColor, to.fogColor, t);
            RenderSettings.fogDensity = Mathf.Lerp(from.fogDensity, to.fogDensity, t);
        }
        else
        {
            RenderSettings.fog = false;
        }

        // ��������
        if (windZone)
        {
            windZone.windMain = Mathf.Lerp(from.windStrength, to.windStrength, t);
        }
    }

    private void ApplyWeather(WeatherPreset preset)
    {
        if (preset == null)
        {
            Debug.LogError("Cannot apply weather. WeatherPreset is null!");
            return;
        }

        Debug.Log("Applying weather preset: " + preset.name);
       // Debug.Log("Enable Leaves: " + preset.enableLeaves);

        Debug.Log($"Applying weather preset: {preset.name}");

        // ��պк͹�������
        RenderSettings.skybox = preset.skybox;
        DynamicGI.UpdateEnvironment();

        if (directionalLight)
        {
            directionalLight.color = preset.lightColor;
            directionalLight.intensity = preset.lightIntensity;
        }

        RenderSettings.ambientLight = preset.ambientColor;
        RenderSettings.ambientIntensity = preset.ambientIntensity;

        RenderSettings.fog = preset.enableFog;
        if (preset.enableFog)
        {
            RenderSettings.fogColor = preset.fogColor;
            RenderSettings.fogDensity = preset.fogDensity;
        }

        if (windZone)
        {
            windZone.windMain = preset.windStrength;
            Vector3 randomWindDirection = Quaternion.Euler(0, Random.Range(0, preset.windRandomRange), 0) * Vector3.forward;
            windZone.transform.rotation = Quaternion.LookRotation(randomWindDirection);
        }

        //if (leafSpawner != null)
       // {
            //if (preset.enableLeaves)
           // {
                //leafSpawner.ConfigureLeaves(preset.leafDuration, preset.minLeafInterval, preset.maxLeafInterval, preset.leafParticleCount);
                //leafSpawner.StartLeafEffect();
           // }
            //else
            //{
                //leafSpawner.StopLeafEffect();
            //}
       // }

       

        // ���������Ӻ�Ripple����
        if (rainSpawner)
        {
            if (preset.enableRain)
            {
                rainSpawner.SpawnRain(new Vector3(0, 34.8f, 0)); // ������
                rainSpawner.SpawnRipple(new Vector3(0, 0, 0));   // Ripple
            }
            else
            {
                rainSpawner.DisableRain();
            }
        }

        // ���ֽ���
        StartCoroutine(FadeInAudio(preset.weatherSound));
        Debug.Log($"Weather preset {preset.name} applied successfully.");
    }

    private IEnumerator FadeInAudio(AudioClip newClip)
    {
        if (audioSource == null) yield break;

        // ������ǰ��Ч
        while (audioSource.volume > 0)
        {
            audioSource.volume -= Time.deltaTime;
            yield return null;
        }
        audioSource.Stop();

        // �л�����������Ч
        if (newClip != null)
        {
            audioSource.clip = newClip;
            audioSource.Play();
            while (audioSource.volume < 1)
            {
                audioSource.volume += Time.deltaTime;
                yield return null;
            }
        }
    }
}
