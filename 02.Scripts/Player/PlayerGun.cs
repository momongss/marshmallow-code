using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : Player
{
    [Header("Player Gun")]
    public Gun gun;

    protected override void Awake()
    {
        base.Awake();

        maxHP = stat.maxHP;
        gun.damage = stat.damage;
        gun.fireRate = stat.fireRate;
        gun.knockbackForce = stat.knockbackForce;
    }

    protected override void Aim(Transform target)
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance < gun.shootRange * 1.5f)
        {
            base.Aim(target);
            gun.AutoAim(target);
        } else
        {
            Rotation(moveDir, moveDir);
        }
    }

    protected override void Attack(Transform target)
    {
        base.Attack(target);

        float distance = Vector3.Distance(target.position, transform.position);

        if (gun.shootRange >= distance)
        {
            Gun.FireState fireState = gun.Fire(target, feverEnforceDamage, feverEnforceFireRate);

            switch (fireState)
            {
                case Gun.FireState.Fired:

                    break;

                case Gun.FireState.Reloading:

                    break;

                case Gun.FireState.Missed:
                    break;

                default:
                    Debug.LogError($"정의되지 않은 FireState {fireState}");
                    break;
            }
        }
    }
}
