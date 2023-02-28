using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    const int UPPER = 0;
    const int LOWER = 1;

    [SerializeField] Animator[] animatorsList;

    public float walkSpeed;

    [SerializeField] private GameObject upperBody;
    [SerializeField] private GameObject lowerBody;
    [SerializeField] Transform leftLeg, rightLeg;
    [SerializeField] GameObject footPrint;
    public float footPrintSinker = 0.07f;

    [SerializeField] Joystick moveJoystick;
    
    public float turningRate = 1f;

    [SerializeField] Rigidbody rigid;

    bool isRunning = false;

    private void Update()
    {
        Move();
    }

    void Move()
    {
        float joyStickX = moveJoystick.Horizontal;
        float joyStickZ = moveJoystick.Vertical;

        if (joyStickX == 0 && joyStickZ == 0)
        {
            stopRunningAnimation();
            return;
        }

        Vector3 moveHorizontal = GetCamRight() * joyStickX;
        Vector3 moveVertical = GetCamForward() * joyStickZ;

        Vector3 moveDir = (moveHorizontal + moveVertical).normalized;

        startRunningAnimation(RunDir.forward);
        rigid.MovePosition(transform.position + moveDir * Time.deltaTime * walkSpeed);

        if (moveDir != Vector3.zero)
        {
            Vector3 newDirUpper = Vector3.RotateTowards(upperBody.transform.forward, moveDir, turningRate * Time.deltaTime, 0.0f);
            Vector3 newDirLower = Vector3.RotateTowards(lowerBody.transform.forward, moveDir, turningRate * Time.deltaTime, 0.0f);
            upperBody.transform.rotation = Quaternion.LookRotation(newDirUpper);
            lowerBody.transform.rotation = Quaternion.LookRotation(newDirLower);
        }
    }

    Vector3 GetCamForward()
    {
        Vector3 camForward = Camera.main.transform.forward;
        camForward.y = 0;

        camForward = camForward.normalized;

        return camForward;
    }

    Vector3 GetCamRight()
    {
        Vector3 camRight = Camera.main.transform.right;
        camRight.y = 0;

        camRight = camRight.normalized;

        return camRight;
    }

    enum RunDir { forward, backward, right, left };

    void startRunningAnimation(RunDir runState)
    {
        if (isRunning) return;
        animatorsList[LOWER].SetTrigger("Run");

        isRunning = true;
    }

    void stopRunningAnimation()
    {
        if (!isRunning) return;
        animatorsList[LOWER].SetTrigger("Idle");
        isRunning = false;
    }
}
