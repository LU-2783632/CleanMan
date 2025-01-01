using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafSpawner : MonoBehaviour
{
    public ParticleSystem leafParticleSystem;    // 粒子系统
    public LeafSpawnerWithPool leafSpawnerPool; // 对象池管理器
    public Vector3 minScale = new Vector3(5f, 5f, 5f); // 最小尺寸
    public Vector3 maxScale = new Vector3(8f, 8f, 8f); // 最大尺寸

    void Update()
    {
        // 获取粒子数据
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[leafParticleSystem.particleCount];
        int count = leafParticleSystem.GetParticles(particles);

        for (int i = 0; i < count; i++)
        {
            // 检测粒子的剩余生命周期
            if (particles[i].remainingLifetime < 0.1f) // 粒子即将消失
            {
                // 粒子的位置直接使用
                Vector3 position = particles[i].position; // 直接获取粒子位置（世界坐标）
                Quaternion rotation = Quaternion.Euler(0, Random.Range(0, 360), 0); // 随机旋转

                // 从对象池中获取一个叶子
                GameObject leaf = leafSpawnerPool.GetLeaf();
                if (leaf == null)
                {
                    Debug.LogError("Failed to get leaf from pool.");
                    continue;
                }

                leaf.transform.position = position;
                leaf.transform.rotation = rotation;

                // 设置随机大小
                Vector3 randomScale = new Vector3(
                    Random.Range(minScale.x, maxScale.x),
                    Random.Range(minScale.y, maxScale.y),
                    Random.Range(minScale.z, maxScale.z)
                );
                leaf.transform.localScale = randomScale;

                // 确保附加的脚本
                //if (!leaf.TryGetComponent<GoodswithBroom>(out _))
                //{
                   // leaf.AddComponent<GoodswithBroom>();
                //}
                //if (!leaf.TryGetComponent<Outline>(out _))
                //{
                   // leaf.AddComponent<Outline>();
                //}

                // 重置粒子的生命周期，让它消失
                particles[i].remainingLifetime = 0;

                // 启动一个计时器，自动回收叶子到池中
                //StartCoroutine(ReturnLeafToPoolAfterDelay(leaf, 5f)); // 5 秒后回收
            }
        }

        // 更新粒子系统
        leafParticleSystem.SetParticles(particles, count);
    }

    private IEnumerator ReturnLeafToPoolAfterDelay(GameObject leaf, float delay)
    {
        yield return new WaitForSeconds(delay);
        leafSpawnerPool.ReturnLeaf(leaf);
    }
}

