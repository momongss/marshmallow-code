using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOrigin : MonoBehaviour
{
    public static PlayerOrigin I { get; private set; }

    [SerializeField] Joystick joystick;
    Transform camTransform;

    public float moveSpeed = 1.2f;
    public bool isMoving = false;
    public Vector3 moveDir;

    private void Awake()
    {
        I = this;

        camTransform = Camera.main.transform;
    }

    private void Update()
    {
        float joyStickX = joystick.Horizontal;
        float joyStickZ = joystick.Vertical;

        if (joyStickX == 0 && joyStickZ == 0)
        {
            isMoving = false;
        } else
        {            
            moveDir = (
            Utils.GetHorizontalDir(camTransform) * joyStickX +
            Utils.GetVerticalDir(camTransform) * joyStickZ)
            .normalized;

            transform.position = transform.position + moveDir * Time.deltaTime * moveSpeed;

            isMoving = true;
        }
    }
}
