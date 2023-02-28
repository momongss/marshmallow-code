using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float damage = 50f;
    public float knockbackForce = 5f;

    public Collider coll;

    public ParticleSystem PS_explosion;

    private void Awake()
    {
        coll.enabled = false;
    }

    public void Burst()
    {
        coll.enabled = true;
        PS_explosion.Clear();
        PS_explosion.Play();

        StartCoroutine(Stop());
    }

    IEnumerator Stop()
    {
        yield return new WaitForSeconds(0.1f);
        coll.enabled = false;

        PoolGeneral.I._Destroy(this, 4f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Layer.ENEMY)
        {
            Enemy p = other.gameObject.GetComponent<Enemy>();
            p.OnDamaged(transform.position, p.transform.position - transform.position, damage, knockbackForce, 0f);
        }
    }
}
