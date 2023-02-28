using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Layer.ENEMY)
        {
            other.gameObject.GetComponent<Enemy>().OnDamaged(Vector3.zero, Vector3.zero, Mathf.Infinity, 0f, 0f);
        }
    }
}
