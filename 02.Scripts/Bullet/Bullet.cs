using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public ParticleSystem PS_muzzleFire;
    public ParticleSystem PS_hit;

    public Explosion explosion;
    public int damage;

    public float speed;
    public float knockbackForce;

    public Rigidbody rigid;

    private void FixedUpdate()
    {
        rigid.MovePosition(rigid.position + transform.forward * speed * Time.deltaTime);
    }

    public void Fire()
    {
        if (PS_muzzleFire == null)
        {
            return;
        }

        ParticleSystem psMuzzle = (ParticleSystem)PoolGeneral.I._Instantiate(PS_muzzleFire, transform.position, transform.rotation);
        psMuzzle.Play();

        PoolGeneral.I._Destroy(psMuzzle, 1.2f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Layer.TERRAIN)
        {
            if (explosion != null)
            {
                Explosion _explosion = (Explosion)PoolGeneral.I._Instantiate(explosion, transform.position, transform.rotation);
                _explosion.damage = damage;
                _explosion.knockbackForce = knockbackForce;
                _explosion.Burst();

                SoundManager.I.Explosion02.Play();

                PoolGeneral.I._Destroy(_explosion, 0.5f);
            } else
            {
                ParticleSystem _bulletHit = (ParticleSystem)PoolGeneral.I._Instantiate(PS_hit, transform.position, transform.rotation);
                PoolGeneral.I._Destroy(_bulletHit, 2f);

                Enemy enemy = other.GetComponent<Enemy>();

                if (enemy)
                {
                    enemy.OnDamaged(
                        transform.position,
                        enemy.transform.position - transform.position,
                        damage,
                        knockbackForce,
                        0f);
                }
            }
            
            PoolGeneral.I._Destroy(this);
        }
    }
}
