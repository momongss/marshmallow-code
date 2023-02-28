using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : MonoBehaviour
{
    public enum Type
    {
        HP = 0,
        Speed = 1,
        Power = 2,
    }

    public Type type;

    public float ratio, duration;

    public virtual void Spawn(float ratio, float duration)
    {
        this.ratio = ratio;
        this.duration = duration;

        print($"R {type} {ratio}");
    }

    public virtual void Take()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Layer.PLAYER)
        {
            Take();

            Destroy(gameObject);
            // PoolGeneral.I._Destroy(this);
        }
    }
}
