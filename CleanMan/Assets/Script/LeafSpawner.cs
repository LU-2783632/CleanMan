using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafSpawner : MonoBehaviour
{
    public ParticleSystem leafParticleSystem;    // ����ϵͳ
    public LeafSpawnerWithPool leafSpawnerPool; // ����ع�����
    public Vector3 minScale = new Vector3(5f, 5f, 5f); // ��С�ߴ�
    public Vector3 maxScale = new Vector3(8f, 8f, 8f); // ���ߴ�

    void Update()
    {
        // ��ȡ��������
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[leafParticleSystem.particleCount];
        int count = leafParticleSystem.GetParticles(particles);

        for (int i = 0; i < count; i++)
        {
            // ������ӵ�ʣ����������
            if (particles[i].remainingLifetime < 0.1f) // ���Ӽ�����ʧ
            {
                // ���ӵ�λ��ֱ��ʹ��
                Vector3 position = particles[i].position; // ֱ�ӻ�ȡ����λ�ã��������꣩
                Quaternion rotation = Quaternion.Euler(0, Random.Range(0, 360), 0); // �����ת

                // �Ӷ�����л�ȡһ��Ҷ��
                GameObject leaf = leafSpawnerPool.GetLeaf();
                if (leaf == null)
                {
                    Debug.LogError("Failed to get leaf from pool.");
                    continue;
                }

                leaf.transform.position = position;
                leaf.transform.rotation = rotation;

                // ���������С
                Vector3 randomScale = new Vector3(
                    Random.Range(minScale.x, maxScale.x),
                    Random.Range(minScale.y, maxScale.y),
                    Random.Range(minScale.z, maxScale.z)
                );
                leaf.transform.localScale = randomScale;

                // ȷ�����ӵĽű�
                //if (!leaf.TryGetComponent<GoodswithBroom>(out _))
                //{
                   // leaf.AddComponent<GoodswithBroom>();
                //}
                //if (!leaf.TryGetComponent<Outline>(out _))
                //{
                   // leaf.AddComponent<Outline>();
                //}

                // �������ӵ��������ڣ�������ʧ
                particles[i].remainingLifetime = 0;

                // ����һ����ʱ�����Զ�����Ҷ�ӵ�����
                //StartCoroutine(ReturnLeafToPoolAfterDelay(leaf, 5f)); // 5 ������
            }
        }

        // ��������ϵͳ
        leafParticleSystem.SetParticles(particles, count);
    }

    private IEnumerator ReturnLeafToPoolAfterDelay(GameObject leaf, float delay)
    {
        yield return new WaitForSeconds(delay);
        leafSpawnerPool.ReturnLeaf(leaf);
    }
}

