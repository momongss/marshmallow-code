using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : Enemy
{
    [SerializeField] HPBar hpBar;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void Init(int level)
    {
        base.Init(level);

        hpBar.SetMaxHP(maxHP);
    }

    public override void OnDamaged(Vector3 collisionPos, Vector3 collisionDir, float damage, float knockbackForce, float effectDuration)
    {
        base.OnDamaged(collisionPos, collisionDir, damage, knockbackForce, effectDuration);

        hpBar.SetHP(HP);
    }
}
