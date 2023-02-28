using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class Enemy : HPObject
{
    public enum Type
    {
        e_01 = 0,
        e_02 = 1,
        e_gold = 2,
    }

    public Type type;

    public enum State
    {
        Move = 0,
        Attack = 1,
        Die = 2,
        Stunned = 3,
        Skill = 4,
        Win = 5
    }

    public State state;

    public float moveSpeed;
    public float value;
    public float attackPower;
    public float pushPower;
    public Player targetPlayer;
    public Animator animator;
    public DamageText damageText;

    [SerializeField] HPBar hpbar;
    [SerializeField] UIReaction reaction;
    [SerializeField] TextMeshProUGUI text_level;

    private void OnValidate()
    {
        reaction = GetComponentInChildren<UIReaction>();
        animator = GetComponentInChildren<Animator>();
    }

    protected virtual void Awake()
    {
        
    }

    public Player SetNewTarget()
    {
        Player _player = PlayerSquad.I.GetRandomPlayer();

        if (_player == null)
        {
            targetPlayer = null;
            return null;
        }

        targetPlayer = _player;

        return _player;
    }

    IEnumerator SearchNewTargetPlayer()
    {
        while (true)
        {
            if (targetPlayer == null || targetPlayer.state == Player.State.Die)
            {
                SetNewTarget();
            }
            yield return new WaitForSeconds(2f);
        }
    }

    public void ChangeState(State newState)
    {
        state = newState;

        switch (state)
        {
            case State.Move:
                isActive = true;
                animator.SetTrigger("Move");
                break;
            case State.Attack:
                isActive = true;
                break;
            case State.Die:
                isActive = false;
                reaction.Say("아야..");
                animator.SetTrigger("Die");
                break;
            case State.Stunned:
                break;
            case State.Win:
                float probability = Random.Range(0f, 1f);
                if (probability < 0.1f)
                {
                    reaction.Say("이겼다", 12f);
                }
                break;
            default:
                Debug.LogError($"정의되지 않은 state {state}");
                break;
        }
    }

    protected override void Start()
    {
        base.Start();
    }

    public virtual void Init(int level)
    {
        InitTexture();

        EnemyStats stat = EnemyStats.LoadEnemyStats(type, level);

        moveSpeed = stat.moveSpeed;
        if (Enemy.Type.e_02 == type)
        {
            print("여기");
            print(moveSpeed);
        }

        maxHP = stat.maxHP;
        pushPower = stat.pushPower;
        attackPower = stat.attackPower;
        value = stat.value;
        rigid.mass = stat.mass;

        rigid.velocity = Vector3.zero;

        text_level.SetText($"{level}");

        reaction.Init();

        HP = maxHP;

        if (hpbar)
        {
            hpbar.SetMaxHP(maxHP);
        }

        StartCoroutine(SearchNewTargetPlayer());
        ChangeState(State.Move);
    }

    public override void OnDamaged(Vector3 collisionPos, Vector3 collisionDir, float damage, float knockbackForce, float effectDuration)
    {
        base.OnDamaged(collisionPos, collisionDir, damage, knockbackForce, effectDuration);

        DamageText _damageText = (DamageText)PoolGeneral.I._Instantiate(
            damageText,
            new Vector3(
                transform.position.x + Random.Range(-0.15f, 0.15f),
                transform.position.y + 0.1f + Random.Range(-0.15f, 0.15f),
                transform.position.z + Random.Range(-0.15f, 0.15f)
            ),
            Quaternion.identity
        );
        _damageText.Init($"{Mathf.Floor(damage)}");

        if (hpbar)
        {
            hpbar.SetHP(HP, 2f);
        }

        string dieReactionWord = LocalizationSettings.StringDatabase.GetLocalizedString("CharaterTalk", $"Hit_Reaction_{Random.Range(0, 2)}");

        float r = Random.Range(0, 3);

        if (r == 0) reaction.Say(dieReactionWord, 0.7f);
    }

    void Update()
    {
        switch (state)
        {
            case State.Move:
                Move();
                break;
            case State.Attack:
                break;
            case State.Stunned:
                break;
            case State.Die:
                break;
            case State.Win:
                break;

            default:
                Debug.LogError($"정의되지 않은 state {state}");
                break;
        }

    }

    protected virtual void Move()
    {
        if (targetPlayer == null) return;
        Vector3 moveDir = (targetPlayer.transform.position - transform.position).normalized;

        transform.SetPositionAndRotation(
            transform.position + moveDir * Time.deltaTime * moveSpeed,
            Quaternion.LookRotation(moveDir, Vector3.up)
            );
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Layer.PLAYER)
        {
            Player player = other.GetComponent<Player>();
            if (player.state == Player.State.Die) return;

            Vector3 collisionPos = other.transform.position;
            Vector3 collisionDir = other.transform.position - transform.position;
            player.OnDamaged(collisionPos, collisionDir, attackPower, pushPower, 0f);

            PushBack(50f);

            StartCoroutine(Stop(0.2f));
        }
    }

    IEnumerator Stop(float delay)
    {
        ChangeState(State.Stunned);

        yield return new WaitForSeconds(delay);

        ChangeState(State.Move);
    }

    public int coinSpawnCount = 1;

    protected override void OnDie()
    {
        base.OnDie();

        SoundManager.I.PlayRandom(SoundManager.I.CuteHits);

        ChangeState(State.Die);

        EnemySquadManager.I.KillEnemy(this, value);

        if (type == Type.e_02)
        {
            coinSpawnCount = 50;
        }
        EnemySquadManager.I.SpawnCoin(transform.position, transform.rotation, value, coinSpawnCount);

        StartCoroutine(_Die());
    }

    IEnumerator _Die()
    {
        yield return new WaitForSeconds(1f);
        PoolGeneral.I._Destroy(this);
    }
}