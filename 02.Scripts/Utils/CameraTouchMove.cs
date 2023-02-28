using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraTouchMove : MonoBehaviour
{
    [SerializeField] Transform cameraRig;

    public float speed = 1f;

    bool isFirstTouch = true;
    Vector3 prevPoint;

    void Update()
    {
        if (Input.GetMouseButton(0) || Input.touchCount > 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << Layer.TERRAIN))
            {
                if (!isFirstTouch)
                {
                    cameraRig.Translate(-(hit.point - prevPoint) * Time.deltaTime * speed);
                }

                prevPoint = hit.point;
                isFirstTouch = false;
            }
        } else
        {
            isFirstTouch = true;
        }
    }
}
