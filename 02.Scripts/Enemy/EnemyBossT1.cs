using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class EnemyBossT1 : EnemyBoss
{
    [SerializeField] float skillCoolTime = 10f;
    [SerializeField] float jumpForce = 2000f;

    [SerializeField] ParticleSystem PS_jump;
    [SerializeField] ParticleSystem PS_landing;
    [SerializeField] Explosion explosion;

    float prevSkillTime;

    protected override void Awake()
    {
        base.Awake();

        prevSkillTime = 0f;
    }

    IEnumerator UseSkiil()
    {
        yield return new WaitForSeconds(2f);

        Instantiate(PS_jump, transform.position, Quaternion.identity);
        rigid.AddForce(Vector3.up * jumpForce);

        yield return new WaitForSeconds(0.5f);
        transform.position = new Vector3(targetPlayer.transform.position.x, 100f, targetPlayer.transform.position.z);
        yield return new WaitForSeconds(0.5f);

        rigid.AddForce(-Vector3.up * jumpForce * 5f);

        yield return new WaitForSeconds(0.5f);

        SetState(State.Move);
    }

    private void OnTriggerEnter(Collider collision)
    {
        switch (state)
        {
            case State.Move:
                break;
            case State.Skill:
                if (collision.gameObject.layer == Layer.TERRAIN)
                {
                    transform.rotation = Quaternion.identity;

                    ParticleSystem ps = Instantiate(PS_landing, transform.position, Quaternion.identity);
                    Destroy(ps.gameObject, 4f);

                    explosion.Burst();
                }
                break;
            case State.Die:
                break;
        }
    }

    void SetState(State newState)
    {
        state = newState;

        switch (state)
        {
            case State.Move:
                break;
            case State.Skill:
                StartCoroutine(UseSkiil());

                prevSkillTime = Time.time;
                break;
            case State.Die:
                break;
        }
    }

    private void Update()
    {
        float currTime = Time.time;

        switch (state)
        {
            case State.Move:
                Move();

                if (
                    currTime - prevSkillTime >= skillCoolTime && 
                    Vector3.Distance(transform.position, targetPlayer.transform.position) > Random.Range(2f, 4f))
                {
                    SetState(State.Skill);
                }
                break;
            case State.Skill:
                break;
            case State.Die:
                break;
        }
    }

    
}
