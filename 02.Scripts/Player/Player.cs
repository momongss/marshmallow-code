using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : HPObject
{
    [Header("Player")]
    public int id;
    public string playerName;

    public string introduction;
    public string[] selfTalks;

    public enum Type
    {
        Pistol = 0,
        Rifle = 1,
        ShotGun = 2,
        Granade = 3,
        Magic = 4,
        Bazooka = 5,
    }

    public Type type;
    public int level;

    public enum State {
        Battle = 0, 
        Die = 1,
        Rest = 2,
        Grabbed = 3,
    };

    public enum RestState
    {
        RollAround = 0,
        Bruise = 1,
        Sitting = 2,
    }

    public State state = State.Battle;
    public RestState restState = RestState.Sitting;

    protected const int UPPER = 0;
    protected const int LOWER = 1;
    protected const int Model = 2;
    protected const int GUN = 3;

    [Header("Body")]

    [SerializeField] private Transform upperBody;
    [SerializeField] private Transform lowerBody;
    [SerializeField] Transform leftLeg, rightLeg;
    [SerializeField] GameObject footPrint;
    public float footPrintSinker = 0.07f;

    [SerializeField] SphereCollider damageCollider;
    [SerializeField] ParticleSystem PS_powerUp;
    [SerializeField] ParticleSystem PS_fever;


    [Header("GamePad")]

    [SerializeField] HPBar hpBar;

    [Header("Ability")]
    public float walkSpeed;
    public float walkSpeed_RestMode = 0.8f;

    // Animation
    [SerializeField] protected Animator[] animatorsList;

    [Header("etc")]
    [SerializeField] protected Talk talk;

    [Header("Sinker")]
    public float animationSpeed;
    public float turningSpeed = 7f;

    // state
    protected bool isAiming = false;

    bool footPrintTurn = true;
    Transform[] footPrintList;

    public Mergable mergable;

    public PlayerStat stat;

    public float feverEnforceDamage = 1f;
    public float feverEnforceFireRate = 1f;

    [SerializeField] TextMeshProUGUI text_level;

    float originWalkSpeed;

    public void SpeedUP(float ratio)
    {
        originWalkSpeed = walkSpeed;
        walkSpeed *= ratio;

        print($"{walkSpeed} {ratio}");
    }

    public void EndSpeedUP()
    {
        walkSpeed = originWalkSpeed;
    }

    public void StartFever(SkillFeverManager.FeverData enforce)
    {
        feverEnforceDamage = enforce.damage;
        feverEnforceFireRate = enforce.fireRate;

        PS_fever.Play();

        talk.Say(Talk.EType.Fever, enforce.feverTime);
    }

    public void EndFever()
    {
        PS_fever.Stop();

        feverEnforceDamage = 1f;
        feverEnforceFireRate = 1f;
    }

    private void OnValidate()
    {
        if (damageCollider == null)
        {
            damageCollider = GetComponent<SphereCollider>();
        }
    }

    public void PowerUp()
    {
        talk.Say(talk.talk_happy);
        PS_powerUp.Play();
    }

    public override void OnDamaged(Vector3 collisionPos, Vector3 collisionDir, float damage, float knockbackForce, float effectDuration)
    {
        base.OnDamaged(collisionPos, collisionDir, damage, knockbackForce, effectDuration);

        hpBar.SetHP(HP);
    }

    protected virtual void Awake()
    {
        stat = PlayerStat.GetStat(type, level);
    }

    protected override void Start()
    {
        base.Start();

        prevRunState = RunState.Idle;

        for (int i = 0; i < animatorsList.Length; i++)
        {
            animatorsList[i].speed = walkSpeed * animationSpeed;
        }

        SoundManager.I.Grab01.Play();
        SoundManager.I.Grab02.Play();
    }

    public void ChangeState(State newState)
    {
        HP = maxHP;
        hpBar.SetMaxHP(HP);

        switch (newState)
        {
            case State.Battle:
                animatorsList[Model].enabled = false;
                animatorsList[UPPER].enabled = true;
                animatorsList[LOWER].enabled = true;

                isActive = true;
                damageCollider.radius = 0.13f;

                talk.Say(Talk.EType.BattleStart, 6f);

                mergable.enabled = false;
                hpBar.gameObject.SetActive(true);

                text_level.transform.parent.gameObject.SetActive(false);

                StartCoroutine(BattleRoutine());

                break;

            case State.Rest:
                animatorsList[Model].enabled = false;
                animatorsList[UPPER].enabled = true;
                animatorsList[LOWER].enabled = true;

                boundary = new Boundary(-1.7f, 1.7f, -3.6f, 5.33f);
                damageCollider.radius = 0.2f;

                hpBar.gameObject.SetActive(false);

                mergable.enabled = true;

                StartCoroutine(RestRoutine());
                text_level.transform.parent.gameObject.SetActive(true);
                text_level.SetText($"{level}");
                break;

            case State.Grabbed:
                talk.Say(Talk.EType.Grabbed, 3f);
                StartCoroutine(ForceResting());
                break;

            default:
                Debug.LogError($"Ã³¸®µÇÁö ¾ÊÀº state {newState}");
                break;
        }

        state = newState;
    }

    int footPrintIndex = 0;

    public PlayerPosition placeBattle;
    public PlayerPosition placeRest;

    public void SetRestPlace(PlayerPosition t)
    {
        t.isAssigned = true;
        placeRest = t;
    }

    public void SetBattlePlace(PlayerPosition t)
    {
        t.isAssigned = true;
        placeBattle = t;
    }

    IEnumerator FootPrint()
    {
        while (true)
        {
            if (true)
            {

                RaycastHit hit;
                Vector3 down = transform.TransformDirection(Vector3.down);

                if (Physics.Raycast(transform.position, down, out hit, 0.5f, 1 << Layer.TERRAIN))
                {
                    if (footPrintTurn)
                    {
                        // left
                        footPrintList[footPrintIndex].position = new Vector3(leftLeg.position.x, hit.point.y + 0.005f, leftLeg.position.z);
                    }
                    else
                    {
                        // right
                        footPrintList[footPrintIndex].position = new Vector3(rightLeg.position.x, hit.point.y + 0.005f, rightLeg.position.z);
                    }
                    footPrintTurn = !footPrintTurn;
                    footPrintIndex++;
                    if (footPrintIndex >= footPrintList.Length)
                    {
                        footPrintIndex = 0;
                    }
                }
            }

            yield return new WaitForSeconds(walkSpeed * footPrintSinker);
        }
    }

    protected virtual void Update()
    {
        switch (state)
        {
            case State.Battle:
                Enemy closestEnemy = EnemySquadManager.I.FindClosestEnemy(transform.position);
                if (closestEnemy != null)
                {
                    isAiming = true;
                    Aim(closestEnemy.transform);
                } else
                {
                    isAiming = false;
                }
                break;
            case State.Die:
                break;
        }
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            case State.Battle:
                Move_BattleMode();
                break;
            case State.Rest:
                if (!isForceResting)
                {
                    MoveTo_RestMode(restModeDestination);
                }
                break;
            case State.Die:
                break;
        }
    }

    enum RunState { 
        forward = 0, 
        backward = 1, 
        right = 2,
        left = 3, 
        Idle = 4 };
    RunState prevRunState;

    void RunningAnimation(bool isRunning)
    {
        if (!isRunning)
        {
            if (prevRunState != RunState.Idle)
            {
                prevRunState = RunState.Idle;
                animatorsList[LOWER].SetTrigger("Idle");
            }
        } else {
            if (prevRunState == RunState.forward) return;

            prevRunState = RunState.forward;
            animatorsList[LOWER].SetTrigger("forward");
        }
    }

    Vector3 restModeDestination;

    [SerializeField] Transform boundaryTransform;
    Boundary boundary;
    
    struct Boundary
    {
        float xL, xS, zL, zS;

        public Boundary(float xL, float xS, float zL, float zS)
        {
            this.xL = xL;
            this.xS = xS;
            this.zL = zL;
            this.zS = zS;
        }

        public Vector3 GetRandomPositionInBoundary()
        {
            float x = Random.Range(xS, xL);
            float z = Random.Range(zS, zL);

            return new Vector3(x, 0f, z);
        }
    }

    IEnumerator ForceResting()
    {
        restModeDestination = transform.position;
        isForceResting = true;
        yield return new WaitForSeconds(5f);
        isForceResting = false;
    }

    bool isForceResting = false;

    IEnumerator BattleRoutine()
    {
        talk.Say(Talk.talk_battleStart, 4);

        while (state == State.Battle)
        {
            float t = Random.Range(3f, 10f);
            yield return new WaitForSeconds(t);

            if (HP > maxHP * 0.4f)
            {
                talk.Say(Talk.talk_battle, t);
            }
            else
            {
                if (t < 7f)
                {
                    talk.Say(Talk.talk_HPLow, t);
                }
            }

            yield return new WaitForSeconds(Random.Range(1f, 2f));
        }
    }

    IEnumerator RestRoutine()
    {
        restModeDestination = transform.position;
        yield return new WaitForSeconds(Random.Range(3f, 10f));

        while (state == State.Rest)
        {
            int r = Random.Range(0, 2);
            if (r == 0)
            {
                restModeDestination = boundary.GetRandomPositionInBoundary();
            }
            else
            {
                restModeDestination = transform.position;

                int talkP = Random.Range(0, 2);
                if (talkP == 0)
                {
                    talk.Say(selfTalks, 5);
                }
            }

            yield return new WaitForSeconds(Random.Range(3f, 10f));
        }
    }

    protected Vector3 moveDir;

    void MoveTo_RestMode(Vector3 destination)
    {
        float distance = Vector3.Distance(destination, transform.position);
        if (distance > 0.05f)
        {
            RunningAnimation(true);
            moveDir = (destination - transform.position).normalized;

            rigid.MovePosition(rigid.position + moveDir * Time.deltaTime * walkSpeed_RestMode);

            Rotation(moveDir, moveDir);
        }
        else
        {
            RunningAnimation(false);
        }
    }

    void MoveTo(Vector3 destination)
    {
        float distance = Vector3.Distance(destination, transform.position);
        if (distance > 0.05f)
        {
            RunningAnimation(true);
            moveDir = (destination - transform.position).normalized;

            rigid.MovePosition(rigid.position + moveDir * Time.deltaTime * walkSpeed);
        }
        else
        {
            RunningAnimation(false);
        }

        if (!isAiming)
        {
            Rotation(moveDir, moveDir);
        }
    }

    void Move_BattleMode()
    {
        MoveTo(placeBattle.transform.position);
    }

    protected void Rotation(Vector3 lookDir, Vector3 moveDir)
    {
        moveDir = limitRotation(moveDir, 0f);
        Vector3 newDir = Vector3.RotateTowards(lowerBody.forward, moveDir, turningSpeed * Time.deltaTime, 0.0f);

        lowerBody.rotation = Quaternion.LookRotation(newDir);

        lookDir = limitRotation(lookDir, 5f);
        Vector3 newDir2 = Vector3.RotateTowards(upperBody.forward, lookDir, turningSpeed * Time.deltaTime, 0.0f);
        upperBody.rotation = Quaternion.LookRotation(newDir2);
    }
    
    protected Vector3 limitRotation(Vector3 rotation, float limit)
    {
        return new Vector3(
            rotation.x,
            Mathf.Clamp(rotation.y, -limit, limit),
            rotation.z);
    }

    protected virtual void Aim(Transform target)
    {
        Vector3 aimDir = target.position - upperBody.position;

        Rotation(aimDir, moveDir);

        Attack(target);
    }

    protected virtual void Attack(Transform target)
    {

    }

    protected override void OnDie()
    {
        talk.Say("²ô¿¡¿¨..");

        animatorsList[Model].enabled = true;
        animatorsList[UPPER].enabled = false;
        animatorsList[LOWER].enabled = false;

        animatorsList[Model].SetInteger("State", 1);

        state = State.Die;
        base.OnDie();

        PlayerSquad.I.OnPlayerDie(this);
    }

    public void Win()
    {
        talk.Say(Talk.talk_battleWin);
        OnEndBattle();
    }

    public void Defeat()
    {
        OnEndBattle();
    }

    public void SetActive(bool active)
    {
        isActive = active;
    }

    void OnEndBattle()
    {
        
    }
}
