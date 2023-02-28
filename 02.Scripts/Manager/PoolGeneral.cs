using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PoolGeneral : MonoBehaviour
{
    public static PoolGeneral I { get; private set; }

    private void Awake()
    {
        I = this;
    }

    Dictionary<int, Queue<Component>> poolDictionary = new Dictionary<int, Queue<Component>>();
    Dictionary<int, int> poolIDDictionary = new Dictionary<int, int>();

    public Component _Instantiate(Component prefab)
    {
        return _Instantiate(prefab, Vector3.zero, Quaternion.identity, null);
    }

    public Component _Instantiate(Component prefab, Transform parent)
    {
        return _Instantiate(prefab, Vector3.zero, Quaternion.identity, parent);
    }

    public Component _Instantiate(Component prefab, Vector3 pos, Quaternion rot, Transform parent = null)
    {
        int prefabID = prefab.GetInstanceID();

        if (!poolDictionary.ContainsKey(prefabID))
        {
            poolDictionary.Add(prefabID, new Queue<Component>());
        }

        Queue<Component> pool = poolDictionary[prefabID];
        if (pool.Count == 0)
        {
            Component instance = Instantiate(prefab, pos, rot, parent);
            int instanceID = instance.GetInstanceID();

            poolIDDictionary.Add(instanceID, prefabID);

            return instance;
        }
        else
        {
            Component instance = pool.Dequeue();
           
            instance.transform.SetPositionAndRotation(pos, rot);
            instance.transform.parent = parent;
            instance.gameObject.SetActive(true);
            return instance;
        }
    }

    public void _Destroy(Component instance, float delay)
    {
        StartCoroutine(_RoutineDestroy(instance, delay));
    }

    IEnumerator _RoutineDestroy(Component instance, float delay)
    {
        yield return new WaitForSeconds(delay);

        _Destroy(instance);
    }

    public void _Destroy(Component instance)
    {
        int instanceID = instance.GetInstanceID();
        int prefabID = poolIDDictionary[instanceID];

        if (!poolDictionary.ContainsKey(prefabID))
        {
            Debug.LogError($"Pool을 통해 생성된 적이 없음. {instance}");
            return;
        }
        
        Queue<Component> pool = poolDictionary[prefabID];
        instance.gameObject.SetActive(false);
        pool.Enqueue(instance);
    }
}
