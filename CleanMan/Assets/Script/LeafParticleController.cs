using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafParticleController : MonoBehaviour
{
    public ParticleSystem leafParticleSystem; // ����ϵͳ
    private float cycleDuration = 130f; // ��ѭ��ʱ��

    void Start()
    {
        // ��ʼѭ������
        StartCoroutine(LeafCycle());
    }

    private IEnumerator LeafCycle()
    {
        while (true)
        {
            // �ȴ����� 30 �룬��������ϵͳ��Rate Over Time Ϊ 1������ 5 ��
            yield return new WaitForSeconds(30f);
            StartLeafEffect(1f, 5f);

            // �ȴ����� 80 �룬��������ϵͳ��Rate Over Time Ϊ 3������ 8 ��
            yield return new WaitForSeconds(50f);
            StartLeafEffect(3f, 8f);

            // �ȴ��� 130 ���������ʼ��һ��ѭ��
            yield return new WaitForSeconds(cycleDuration - 80f);
        }
    }

    private void StartLeafEffect(float rateOverTime, float duration)
    {
        if (leafParticleSystem != null)
        {
            var emission = leafParticleSystem.emission;
            emission.enabled = true; // �������ӷ���
            emission.rateOverTime = rateOverTime; // ���� Rate Over Time
            leafParticleSystem.Play(); // ��ʼ��������ϵͳ

            // ��ָ������ʱ���ر�����ϵͳ
            StartCoroutine(StopLeafEffectAfterDuration(duration));
        }
    }

    private IEnumerator StopLeafEffectAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);

        if (leafParticleSystem != null)
        {
            var emission = leafParticleSystem.emission;
            emission.enabled = false; // �������ӷ���
            leafParticleSystem.Stop(); // ֹͣ����ϵͳ
        }
    }
}
