using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    [SerializeField] private GameObject upperBody;
    [SerializeField] private GameObject lowerBody;

    [SerializeField] Transform destination;
    [SerializeField] Rigidbody rigid;

    [SerializeField] Animator Anim_upper;
    [SerializeField] Animator Anim_lower;

    [SerializeField] Gun gun;

    public float MoveSpeed;
    public float turningRate;

    bool isAttack = false;

    private void Update()
    {
        Move();
        autoAiming();
    }

    bool autoAiming()
    {
        Vector3 aimDir = destination.position - transform.position;
        if (aimDir != Vector3.zero)
        {
            Vector3 upperDir = limitRotationY(aimDir, 0.2f);
            Vector3 lowerDir = limitRotationY(aimDir, 0f);

            Vector3 newDirUpper = Vector3.RotateTowards(upperBody.transform.forward, upperDir, turningRate * Time.deltaTime, 0.0f);
            Vector3 newDirLower = Vector3.RotateTowards(lowerBody.transform.forward, lowerDir, turningRate * Time.deltaTime, 0.0f);
            upperBody.transform.rotation = Quaternion.LookRotation(newDirUpper);
            lowerBody.transform.rotation = Quaternion.LookRotation(newDirLower);
        }

        return true;
    }

    Vector3 limitRotationY(Vector3 rotation, float limit)
    {
        float limitedRotationY = rotation.y;

        if (limitedRotationY > limit) limitedRotationY = limit;
        else if (limitedRotationY < -limit) limitedRotationY = -limit;

        return new Vector3(rotation.x, limitedRotationY, rotation.z);
    }

    void Move()
    {
        Vector3 relativePos = destination.transform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(relativePos, Vector3.up);

        Vector3 moveDir = (destination.transform.position - transform.position).normalized;
        rigid.MovePosition(transform.position + moveDir * Time.deltaTime * MoveSpeed);
    }
}
