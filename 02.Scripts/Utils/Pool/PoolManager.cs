using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager<T> : MonoBehaviour where T : MonoBehaviour
{
    public T prefab;
    Queue<T> queue = new Queue<T>();

    protected virtual T __Instantiate(T obj, Vector3 pos, Quaternion rot)
    {
        T _obj = MonoBehaviour.Instantiate<T>(obj, pos, rot);

        return _obj;
    }

    public virtual T _Instantiate(Vector3 pos, Quaternion rot)
    {
        if (queue.Count == 0)
        {
            T _obj = __Instantiate(prefab, pos, rot);
            return _obj;
        }
        else
        {
            T _obj = queue.Dequeue();
            _obj.transform.position = pos;
            _obj.transform.rotation = rot;
            _obj.gameObject.SetActive(true);
            return _obj;
        }
    }

    public T _Instantiate(T obj, Vector3 pos, Quaternion rot)
    {
        if (queue.Count == 0)
        {
            T _obj = __Instantiate(obj, pos, rot);
            return _obj;
        }
        else
        {
            T _obj = queue.Dequeue();
            _obj.transform.position = pos;
            _obj.transform.rotation = rot;
            _obj.gameObject.SetActive(true);
            return _obj;
        }
    }

    public void Destroy(T obj)
    {
        obj.gameObject.SetActive(false);

        queue.Enqueue(obj);
    }
}
