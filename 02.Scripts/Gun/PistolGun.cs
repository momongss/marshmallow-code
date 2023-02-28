using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolGun : Gun
{
    public override FireState Fire(Transform target, float enforce_damage = 1f, float enforce_fireRate = 1f)
    {
        transform.LookAt(target); // 사격시 정확히 적을 바라보기 위함.
        bool isReadyToFire = FireCommonProcess(enforce_fireRate);

        if (isReadyToFire)
        {
            RaycastHit hit;

            Vector3 fireDir = (target.position - firePos.position).normalized;

            if (Physics.Raycast(firePos.position, fireDir, out hit, shootRange, 1 << Layer.ENEMY))
            {
                Enemy enemy = target.GetComponent<Enemy>();

                if (enemy)
                {
                    float distance = Vector3.Distance(target.position, firePos.position);

                    if (distance > shootRange)
                    {
                        return FireState.Missed;
                    }

                    enemy.OnDamaged(
                        hit.point,
                        fireDir,
                        damage * enforce_damage,
                        knockbackForce,
                        0f);
                }

                ParticleSystem _bulletHit = (ParticleSystem)PoolGeneral.I._Instantiate(bullet.PS_hit, hit.point, transform.rotation);
                _bulletHit.Stop();
                _bulletHit.Play();
                PoolGeneral.I._Destroy(_bulletHit, 2f);
            }

            return FireState.Fired;
        }
        else
        {
            return FireState.Reloading;
        }
    }
}
