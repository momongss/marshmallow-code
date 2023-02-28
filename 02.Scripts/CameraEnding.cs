using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEnding : MonoBehaviour
{
    Camera endingCamera;
    public float moveSpeed;
    public float rotateSpeed;
    public float resizeSpeed;
    public float zoomCameraSize;

    private void Start()
    {
        endingCamera = Camera.main;
    }

    //void Update()
    //{
    //    endingCamera.transform.position = Vector3.MoveTowards(endingCamera.transform.position, transform.position, moveSpeed * Time.deltaTime);
    //    endingCamera.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(endingCamera.transform.forward, transform.forward, rotateSpeed * Time.deltaTime, 0.0f));

    //    if (endingCamera.orthographicSize > zoomCameraSize)
    //    {
    //        endingCamera.orthographicSize -= Time.deltaTime * resizeSpeed;
    //    }
    //}
}
