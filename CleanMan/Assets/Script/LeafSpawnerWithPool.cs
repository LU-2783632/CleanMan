using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafSpawnerWithPool : MonoBehaviour
{
    public GameObject leafPrefab; // 叶子预制体
    public int poolSize = 50;     // 对象池大小

    private Queue<GameObject> leafPool = new Queue<GameObject>();

    void Start()
    {
        // 初始化对象池
        for (int i = 0; i < poolSize; i++)
        {
            GameObject leaf = Instantiate(leafPrefab);
            InitializeLeaf(leaf); // 确保每个叶子在创建时初始化完整
            leaf.SetActive(false);
            leafPool.Enqueue(leaf);


          
        }
    }

    // 从对象池获取一个叶子
    public GameObject GetLeaf()
    {
        GameObject leaf;

        // 如果池中有可用对象
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
            // 如果池中没有对象，动态创建一个新对象
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

        // 启用对象
        leaf.SetActive(true);

        // 初始化对象
        InitializeLeaf(leaf);

        return leaf;
    }

    private void InitializeLeaf(GameObject leaf)
    {
        // 确保 GoodswithBroom 脚本正确初始化
        GoodswithBroom goods = leaf.GetComponent<GoodswithBroom>();
        if (goods == null)
        {
            Debug.LogError($"GoodswithBroom is missing on {leaf.name}");
            return;
        }

        goods.Initialize(); // 确保调用初始化
        Debug.Log($"Initialized {leaf.name} with GoodswithBroom.");


        // 添加或检查 Collider
        if (!leaf.TryGetComponent<Collider>(out Collider collider))
        {
            collider = leaf.AddComponent<BoxCollider>(); // 添加 BoxCollider
        }
        if (collider is BoxCollider boxCollider)
        {
            boxCollider.size = new Vector3(0.1f, 0.02f, 0.1f); // 设置合适的碰撞体尺寸
            boxCollider.center = Vector3.zero;
        }


        // 确保 Outline 存在
        if (!leaf.TryGetComponent<Outline>(out Outline outline))
        {
            outline = leaf.AddComponent<Outline>();
            Debug.Log($"Added Outline to {leaf.name}");
        }
        outline.enabled = false; // 初始禁用 Outline

        // 设置叶子 Layer
        leaf.layer = 11; // GoodsBroom 层
        Debug.Log($"Initialized leaf {leaf.name} with layer: {leaf.layer}");
    }

    // 将叶子返回对象池
    public void ReturnLeaf(GameObject leaf)
    {
        leaf.SetActive(false);
        leafPool.Enqueue(leaf);
    }
}
