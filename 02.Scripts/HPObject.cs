using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPObject : MonoBehaviour
{
    [Header("HPObject")]
    public float maxHP;
    protected float HP;

    [SerializeField] protected Rigidbody rigid;
    protected bool isActive = true;

    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] SkinnedMeshRenderer skinedMeshRenderer;

    [SerializeField] Texture hitTexture;
    Texture mainTexture;
    Material mat;

    protected virtual void Start()
    {
        HP = maxHP;
        rigid = gameObject.GetComponent<Rigidbody>();

        InitTexture();
    }

    protected void InitTexture()
    {
        if (meshRenderer)
        {
            mat = meshRenderer.material;
            mainTexture = mat.mainTexture;
        }
        else if (skinedMeshRenderer)
        {
            mat = skinedMeshRenderer.material;
            mainTexture = mat.mainTexture;
        }
    }

    IEnumerator HitColoring()
    {
        mat.mainTexture = hitTexture;
        yield return new WaitForSeconds(0.15f);
        mat.mainTexture = mainTexture;
    }

    public void Push(float force)
    {
        rigid.AddForce(-transform.forward * force);
    }

    public virtual void OnDamaged(Vector3 collisionPos, Vector3 collisionDir, float damage, float knockbackForce, float effectDuration)
    {
        if (damage == 0) return;

        rigid.AddForce(collisionDir * knockbackForce, ForceMode.Impulse);

        if (isActive)
        {
            HP -= damage;

            if (mat)
            {
                StartCoroutine(HitColoring());
            }

            if (HP <= 0f)
            {
                rigid.AddForce(collisionDir * knockbackForce * 0.5f, ForceMode.Impulse);
                rigid.AddRelativeTorque(transform.forward * knockbackForce, ForceMode.Impulse);
                HP = 0f;
                OnDie();
            }
        }
    }

    public void PushBack(float force)
    {
        rigid.AddForce(-transform.forward * force);
    }

    protected virtual void OnDie()
    {
        isActive = false;
    }
}
