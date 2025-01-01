using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainSpawnerWithPool : MonoBehaviour
{
    public GameObject rainPrefab;       // ������ϵͳ prefab
    public GameObject ripplePrefab;  // Ripple prefab
    public Transform rainParent;        // ������ϵͳ������
    private GameObjectPool _rainPool;   // �����Ӷ����
    public Transform rippleParent;   // Ripple ����ϵͳ������
    private GameObjectPool _ripplePool; // Ripple �����

    private float rainIntensity = 0f; // ��ǿ��
    private float targetRainIntensity = 0f; // Ŀ����ǿ��
    public float intensityChangeSpeed = 0.1f; // ǿ�ȱ仯�ٶ�

    public List<GameObject> activeRipples = new List<GameObject>(); // ��Ծ�� Ripple

    void Start()
    {
        // ��� rainParent �ǳ־û����壬����һ����̬������
        if (rainParent != null && rainParent.gameObject.scene.name == null)
        {
            GameObject dynamicParent = new GameObject("RainPoolParent");
            rainParent = dynamicParent.transform;
        }

        if (rippleParent == null)
        {
            rippleParent = new GameObject("RippleParent").transform;
        }

        // ��ʼ����� Ripple �����
        _rainPool = new GameObjectPool(rainPrefab, rainParent, 10);
        _ripplePool = new GameObjectPool(ripplePrefab, rainParent, 10);
    }

    void Update()
    {
        // ���������ǿ�Ⱥ� Ripple ǿ��
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
                emission.rateOverTime = Mathf.Lerp(0, 100, intensity); // ����ǿ�����÷�������
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
                emission.rateOverTime = Mathf.Lerp(0, 50, intensity); // Ripple �ķ�������
            }
        }
    }

    public void SpawnRain(Vector3 position)
    {
        // �̶� Y ��Ϊ 34.8
        position.y = 34.8f;

        // �Ӷ�����л�ȡ������ϵͳ
        GameObject rain = _rainPool.Get();
        rain.transform.position = position;
        rain.SetActive(true); // ȷ�����󼤻�

        // ��һ��ʱ���黹�������
        //StartCoroutine(ReturnToPoolAfterSeconds(rain, 3f));
    }
    public void SpawnRipple(Vector3 position)
    {
        Vector3 ripplePosition = position;
        ripplePosition.y = 0; // Ripple ��������

        GameObject ripple = _rainPool.Get();
        ripple.transform.position = position;
        ripple.SetActive(true);
        activeRipples.Add(ripple); // ��¼��Ծ��Ripple
    }


    private System.Collections.IEnumerator ReturnToPoolAfterSeconds(GameObject obj, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _rainPool.Return(obj); // �黹�������
    }

    public void DisableAllRainAndRipples()
    {
        // ��������������
        foreach (var rain in _rainPool.GetActiveObjects())
        {
            _rainPool.Return(rain);
        }

        // ��������Ripple
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
        // �������л�Ծ��������
        foreach (var rain in _rainPool.GetActiveObjects().ToArray()) // ʹ�����鸱������
        {
            _rainPool.Return(rain);
        }
    }
}
