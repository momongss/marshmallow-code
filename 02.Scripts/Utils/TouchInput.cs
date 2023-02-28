using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void CallBack();

public class TouchInput : MonoBehaviour
{
    Dictionary<TouchPhase, Dictionary<GameObject, CallBack>> eventCallbackDic = new Dictionary<TouchPhase, Dictionary<GameObject, CallBack>>();

    void Awake()
    {
        eventCallbackDic.Add(TouchPhase.Began, new Dictionary<GameObject, CallBack>());
        eventCallbackDic.Add(TouchPhase.Canceled, new Dictionary<GameObject, CallBack>());
        eventCallbackDic.Add(TouchPhase.Ended, new Dictionary<GameObject, CallBack>());
        eventCallbackDic.Add(TouchPhase.Moved, new Dictionary<GameObject, CallBack>());
        eventCallbackDic.Add(TouchPhase.Stationary, new Dictionary<GameObject, CallBack>());
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                Vector3 touchPosition = touch.position;
                Ray ray = Camera.main.ScreenPointToRay(touchPosition);
                RaycastHit hit;
                Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow, 100f);
                if (Physics.Raycast(ray, out hit))
                {
                    GameObject touchedObject = hit.transform.gameObject;
                    if (eventCallbackDic[touch.phase].ContainsKey(touchedObject))
                    {
                        CallBack callback = eventCallbackDic[touch.phase][touchedObject];
                        callback();
                    }
                }
            }
        }
    }

    public void addEventCallback(TouchPhase touchPhase, GameObject _gameObject, CallBack callback)
    {
        eventCallbackDic[touchPhase].Add(_gameObject, callback);
    }
}
