using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafSpawnerWithPool : MonoBehaviour
{
    public GameObject leafPrefab; // Ҷ��Ԥ����
    public int poolSize = 50;     // ����ش�С

    private Queue<GameObject> leafPool = new Queue<GameObject>();

    void Start()
    {
        // ��ʼ�������
        for (int i = 0; i < poolSize; i++)
        {
            GameObject leaf = Instantiate(leafPrefab);
            InitializeLeaf(leaf); // ȷ��ÿ��Ҷ���ڴ���ʱ��ʼ������
            leaf.SetActive(false);
            leafPool.Enqueue(leaf);


          
        }
    }

    // �Ӷ���ػ�ȡһ��Ҷ��
    public GameObject GetLeaf()
    {
        GameObject leaf;

        // ��������п��ö���
        if (leafPool.Count > 0)
        {
            leaf = leafPool.Dequeue();

            if (leaf == null)
            {
                Debug.LogError("Dequeued leaf is null.");
                return null;
            }
            else
            {
                Debug.Log($"Dequeued leaf: {leaf.name}");
            }
        }
        else
        {
            // �������û�ж��󣬶�̬����һ���¶���
            Debug.LogWarning("Leaf pool is empty. Creating a new leaf.");
            leaf = Instantiate(leafPrefab);

            if (leaf == null)
            {
                Debug.LogError("Failed to instantiate new leaf.");
                return null;
            }
            else
            {
                Debug.Log($"Instantiated new leaf: {leaf.name}");
            }
        }

        // ���ö���
        leaf.SetActive(true);

        // ��ʼ������
        InitializeLeaf(leaf);

        return leaf;
    }

    private void InitializeLeaf(GameObject leaf)
    {
        // ȷ�� GoodswithBroom �ű���ȷ��ʼ��
        GoodswithBroom goods = leaf.GetComponent<GoodswithBroom>();
        if (goods == null)
        {
            Debug.LogError($"GoodswithBroom is missing on {leaf.name}");
            return;
        }

        goods.Initialize(); // ȷ�����ó�ʼ��
        Debug.Log($"Initialized {leaf.name} with GoodswithBroom.");


        // ��ӻ��� Collider
        if (!leaf.TryGetComponent<Collider>(out Collider collider))
        {
            collider = leaf.AddComponent<BoxCollider>(); // ��� BoxCollider
        }
        if (collider is BoxCollider boxCollider)
        {
            boxCollider.size = new Vector3(0.1f, 0.02f, 0.1f); // ���ú��ʵ���ײ��ߴ�
            boxCollider.center = Vector3.zero;
        }


        // ȷ�� Outline ����
        if (!leaf.TryGetComponent<Outline>(out Outline outline))
        {
            outline = leaf.AddComponent<Outline>();
            Debug.Log($"Added Outline to {leaf.name}");
        }
        outline.enabled = false; // ��ʼ���� Outline

        // ����Ҷ�� Layer
        leaf.layer = 11; // GoodsBroom ��
        Debug.Log($"Initialized leaf {leaf.name} with layer: {leaf.layer}");
    }

    // ��Ҷ�ӷ��ض����
    public void ReturnLeaf(GameObject leaf)
    {
        leaf.SetActive(false);
        leafPool.Enqueue(leaf);
    }
}
