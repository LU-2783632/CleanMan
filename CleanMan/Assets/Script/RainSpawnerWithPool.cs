using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainSpawnerWithPool : MonoBehaviour
{
    public GameObject rainPrefab;       // 雨粒子系统 prefab
    public GameObject ripplePrefab;  // Ripple prefab
    public Transform rainParent;        // 雨粒子系统父物体
    private GameObjectPool _rainPool;   // 雨粒子对象池
    public Transform rippleParent;   // Ripple 粒子系统父物体
    private GameObjectPool _ripplePool; // Ripple 对象池

    private float rainIntensity = 0f; // 雨强度
    private float targetRainIntensity = 0f; // 目标雨强度
    public float intensityChangeSpeed = 0.1f; // 强度变化速度

    public List<GameObject> activeRipples = new List<GameObject>(); // 活跃的 Ripple

    void Start()
    {
        // 如果 rainParent 是持久化物体，创建一个动态父物体
        if (rainParent != null && rainParent.gameObject.scene.name == null)
        {
            GameObject dynamicParent = new GameObject("RainPoolParent");
            rainParent = dynamicParent.transform;
        }

        if (rippleParent == null)
        {
            rippleParent = new GameObject("RippleParent").transform;
        }

        // 初始化雨和 Ripple 对象池
        _rainPool = new GameObjectPool(rainPrefab, rainParent, 10);
        _ripplePool = new GameObjectPool(ripplePrefab, rainParent, 10);
    }

    void Update()
    {
        // 渐变控制雨强度和 Ripple 强度
        if (Mathf.Abs(rainIntensity - targetRainIntensity) > 0.01f)
        {
            rainIntensity = Mathf.Lerp(rainIntensity, targetRainIntensity, Time.deltaTime * intensityChangeSpeed);
            UpdateRainIntensity(rainIntensity);
            UpdateRippleIntensity(rainIntensity);
        }
    }

    public void SetRainIntensity(float intensity)
    {
        targetRainIntensity = Mathf.Clamp01(intensity);
    }

    private void UpdateRainIntensity(float intensity)
    {
        foreach (var rain in _rainPool.GetActiveObjects())
        {
            ParticleSystem ps = rain.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                var emission = ps.emission;
                emission.rateOverTime = Mathf.Lerp(0, 100, intensity); // 根据强度设置发射速率
            }
        }
    }
    private void UpdateRippleIntensity(float intensity)
    {
        foreach (var ripple in activeRipples)
        {
            ParticleSystem ps = ripple.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                var emission = ps.emission;
                emission.rateOverTime = Mathf.Lerp(0, 50, intensity); // Ripple 的发射速率
            }
        }
    }

    public void SpawnRain(Vector3 position)
    {
        // 固定 Y 轴为 34.8
        position.y = 34.8f;

        // 从对象池中获取雨粒子系统
        GameObject rain = _rainPool.Get();
        rain.transform.position = position;
        rain.SetActive(true); // 确保对象激活

        // 在一定时间后归还到对象池
        //StartCoroutine(ReturnToPoolAfterSeconds(rain, 3f));
    }
    public void SpawnRipple(Vector3 position)
    {
        Vector3 ripplePosition = position;
        ripplePosition.y = 0; // Ripple 贴近地面

        GameObject ripple = _rainPool.Get();
        ripple.transform.position = position;
        ripple.SetActive(true);
        activeRipples.Add(ripple); // 记录活跃的Ripple
    }


    private System.Collections.IEnumerator ReturnToPoolAfterSeconds(GameObject obj, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _rainPool.Return(obj); // 归还到对象池
    }

    public void DisableAllRainAndRipples()
    {
        // 禁用所有雨粒子
        foreach (var rain in _rainPool.GetActiveObjects())
        {
            _rainPool.Return(rain);
        }

        // 禁用所有Ripple
        foreach (var ripple in activeRipples)
        {
            if (ripple.activeSelf)
            {
                _ripplePool.Return(ripple);
            }
        }
        activeRipples.Clear();
    }
    public void DisableRain()
    {
        // 禁用所有活跃的雨粒子
        foreach (var rain in _rainPool.GetActiveObjects().ToArray()) // 使用数组副本迭代
        {
            _rainPool.Return(rain);
        }
    }
}
