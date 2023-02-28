using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManagerGeneral : MonoBehaviour
{
    public GameObject prefab;
    Queue<GameObject> queue = new Queue<GameObject>();

    protected virtual GameObject __Instantiate(GameObject obj, Vector3 pos, Quaternion rot)
    {
        GameObject _obj = MonoBehaviour.Instantiate<GameObject>(obj, pos, rot);

        return _obj;
    }

    public virtual GameObject _Instantiate(Vector3 pos, Quaternion rot)
    {
        if (queue.Count == 0)
        {
            GameObject _obj = __Instantiate(prefab, pos, rot);
            return _obj;
        }
        else
        {
            GameObject _obj = queue.Dequeue();
            _obj.transform.position = pos;
            _obj.transform.rotation = rot;
            _obj.gameObject.SetActive(true);
            return _obj;
        }
    }

    public GameObject _Instantiate(GameObject obj, Vector3 pos, Quaternion rot)
    {
        if (queue.Count == 0)
        {
            GameObject _obj = __Instantiate(obj, pos, rot);
            return _obj;
        }
        else
        {
            GameObject _obj = queue.Dequeue();
            _obj.transform.position = pos;
            _obj.transform.rotation = rot;
            _obj.gameObject.SetActive(true);
            return _obj;
        }
    }

    public void Destroy(GameObject obj)
    {
        obj.gameObject.SetActive(false);

        queue.Enqueue(obj);
    }
}
