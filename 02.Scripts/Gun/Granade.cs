using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granade : MonoBehaviour
{
    public Rigidbody rigid;
    public int damage;

    public float throwForce = 10f;

    public Explosion explosion;

    private void Reset()
    {
        rigid = GetComponent<Rigidbody>();
        if (rigid == null)
        {
            rigid = gameObject.AddComponent<Rigidbody>();
        }
    }

    public void Throw(Vector3 dir)
    {
        rigid.AddForce(dir * throwForce);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Layer.TERRAIN)
        {
            Explosion _explosion = (Explosion)PoolGeneral.I._Instantiate(explosion, transform.position, transform.rotation);
            _explosion.damage = damage;
            _explosion.Burst();

            rigid.velocity = Vector3.zero;
            PoolGeneral.I._Destroy(this);
        }
    }
}
