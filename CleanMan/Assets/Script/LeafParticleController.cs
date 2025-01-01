using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafParticleController : MonoBehaviour
{
    public ParticleSystem leafParticleSystem; // 粒子系统
    private float cycleDuration = 130f; // 总循环时长

    void Start()
    {
        // 开始循环控制
        StartCoroutine(LeafCycle());
    }

    private IEnumerator LeafCycle()
    {
        while (true)
        {
            // 等待到第 30 秒，启动粒子系统，Rate Over Time 为 1，持续 5 秒
            yield return new WaitForSeconds(30f);
            StartLeafEffect(1f, 5f);

            // 等待到第 80 秒，启动粒子系统，Rate Over Time 为 3，持续 8 秒
            yield return new WaitForSeconds(50f);
            StartLeafEffect(3f, 8f);

            // 等待到 130 秒结束，开始新一轮循环
            yield return new WaitForSeconds(cycleDuration - 80f);
        }
    }

    private void StartLeafEffect(float rateOverTime, float duration)
    {
        if (leafParticleSystem != null)
        {
            var emission = leafParticleSystem.emission;
            emission.enabled = true; // 启用粒子发射
            emission.rateOverTime = rateOverTime; // 设置 Rate Over Time
            leafParticleSystem.Play(); // 开始播放粒子系统

            // 在指定持续时间后关闭粒子系统
            StartCoroutine(StopLeafEffectAfterDuration(duration));
        }
    }

    private IEnumerator StopLeafEffectAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);

        if (leafParticleSystem != null)
        {
            var emission = leafParticleSystem.emission;
            emission.enabled = false; // 禁用粒子发射
            leafParticleSystem.Stop(); // 停止粒子系统
        }
    }
}
