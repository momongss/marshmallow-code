using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bazooka : Gun
{
    public override void AutoAim(Transform target)
    {
        Vector3 targetPos = target.position;
        targetPos.y = 0f;

        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetPos - transform.position, turningRate * Time.deltaTime, 0.0f);
        // newDir.y += 0.04f;

        Quaternion fireDir = Quaternion.LookRotation(newDir);
        transform.rotation = fireDir;
    }

    public override FireState Fire(Transform target, float enforce_damage = 1f, float enforce_fireRate = 1f)
    {
        bool isReadyToFire = FireCommonProcess(enforce_fireRate);

        if (isReadyToFire)
        {
            SoundManager.I.Explosion03.Play();

            Bullet _bullet = (Bullet)PoolGeneral.I._Instantiate(bullet, firePos.position, firePos.rotation);
            _bullet.damage = (int)(damage * enforce_damage);
            _bullet.knockbackForce = knockbackForce;

            PoolGeneral.I._Destroy(_bullet, 1f);

            return FireState.Fired;
        }
        else
        {
            return FireState.Reloading;
        }
    }
}
