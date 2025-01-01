using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool
{
    private readonly Queue<GameObject> _pool = new Queue<GameObject>();
    private readonly GameObject _prefab;
    private readonly Transform _parent;
    private List<GameObject> _activeObjects; // ���ڸ��ټ���Ķ���

    public GameObjectPool(GameObject prefab, Transform parent, int initialSize = 10)
    {
        _prefab = prefab;
        _parent = parent;
        _pool = new Queue<GameObject>();
        _activeObjects = new List<GameObject>();


        // ��ʼ����
        for (int i = 0; i < initialSize; i++)
        {
            GameObject obj = Object.Instantiate(_prefab, _parent);
            obj.SetActive(false);
            _pool.Enqueue(obj);
        }
    }

    public GameObject Get()
    {
        GameObject obj;
        if (_pool.Count > 0)
        {
            obj = _pool.Dequeue();
        }
        else
        {
            obj = Object.Instantiate(_prefab, _parent);
        }

        obj.SetActive(true);
        _activeObjects.Add(obj); // ��ӵ������б�
        return obj;
    }

    public void Return(GameObject obj)
    {
        if (!_activeObjects.Contains(obj)) return;

        obj.SetActive(false);
        _activeObjects.Remove(obj); // �Ӽ����б����Ƴ�
        _pool.Enqueue(obj);
    }

    // �����������������м���Ķ���
    public List<GameObject> GetActiveObjects()
    {
        return _activeObjects;
    }
}