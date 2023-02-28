using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicGun : Gun
{
    public override FireState Fire(Transform target, float enforce_damage = 1f, float enforce_fireRate = 1f)
    {
        bool isReadyToFire = FireCommonProcess(enforce_fireRate);

        if (isReadyToFire)
        {
            float distance = Vector3.Distance(target.position, firePos.position);
            if (distance > shootRange)
            {
                return FireState.Missed;
            }

            Bullet _bullet = (Bullet)PoolGeneral.I._Instantiate(bullet, firePos.position, firePos.rotation);
            _bullet.damage = (int)(damage * enforce_damage);
            _bullet.Fire();

            PoolGeneral.I._Destroy(_bullet, 0.5f);

            return FireState.Fired;
        }
        else
        {
            return FireState.Reloading;
        }
    }
}
