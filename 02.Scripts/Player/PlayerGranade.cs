using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGranade : Player
{
    public Granade granade;
    public float attackRange = 2f;

    public float attackRate = 2f;

    float prevAttackTime = 0f;

    [SerializeField] Transform throwPos;

    private void Awake()
    {
        PlayerStat stat = PlayerStat.GetStat(type, level);

        maxHP = stat.maxHP;
        attackRate = stat.fireRate;
        granade.damage = stat.damage;
    }

    protected override void Aim(Transform target)
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance < attackRange * 1.5f)
        {
            base.Aim(target);
        }
        else
        {
            Rotation(moveDir, moveDir);
        }
    }

    IEnumerator Throw(float delay)
    {
        yield return new WaitForSeconds(delay);
        Granade _granade = (Granade)PoolGeneral.I._Instantiate(granade, throwPos.position, transform.rotation);

        _granade.Throw(transform.forward);
    }

    protected override void Attack(Transform target)
    {
        base.Attack(target);

        float distance = Vector3.Distance(target.position, transform.position);

        if (attackRange >= distance)
        {
            float currTime = Time.time;

            if (currTime - prevAttackTime >= attackRate)
            {
                prevAttackTime = currTime;

                animatorsList[UPPER].SetTrigger("Throw");

                StartCoroutine(Throw(0.3f));
            }
        }
    }

}
