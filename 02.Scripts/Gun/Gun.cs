using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    public enum Kind { 
        Pistol = 0, 
        LongGun = 1, 
        ShotGun = 2,
        MagicGun = 3,
        Bazooka = 4,
    };

    public enum State { 
        fire = 0,
        wait = 1,
        reload = 2,
        error = 3
    };

    public Kind kind;

    public Bullet bullet;
    [SerializeField] protected Transform firePos;
    [SerializeField] GameObject gunModel;

    [SerializeField] protected AudioSource gunAudio;

    public int damage;
    public float knockbackForce;
    public float fireRate;
    public float shootRange = 15f;
    public float turningRate;
    public float reboundForce = 2f;

    protected float prevFireTime = 0f;
    protected Coroutine reloadRoutine;

    protected virtual void Awake()
    {
        ResetGun();

        shootRange = 1.8f;

        bullet.knockbackForce = knockbackForce;
    }

    public void ResetGun()
    {
        transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

    public void OnBattleEnd()
    {
        
    }

    public virtual void AutoAim(Transform target)
    {
        Vector3 newDir = Vector3.RotateTowards(transform.forward, target.position - transform.position, turningRate * Time.deltaTime, 0.0f);

        Quaternion fireDir = Quaternion.LookRotation(newDir);
        transform.rotation = fireDir;
    }

    public enum FireState { Fired, Reloading, Missed }

    public virtual FireState Fire(Transform target, float enforce_damage = 1f, float enforce_fireRate = 1f)
    {
        return FireState.Fired;
    }

    protected bool FireCommonProcess(float enforce_fireRate = 1f)
    {
        float currTime = Time.time;

        if (currTime - prevFireTime < Mathf.Max(fireRate / enforce_fireRate, 0.034f))
        {
            return false;
        }

        prevFireTime = currTime;

        if (gunAudio)
        {
            SoundManager.I.PlayGun(gunAudio);
        }

        ParticleSystem _muzzleFire = (ParticleSystem)PoolGeneral.I._Instantiate(bullet.PS_muzzleFire, firePos.position, firePos.rotation);
        _muzzleFire.Play();
        PoolGeneral.I._Destroy(_muzzleFire, 0.5f);

        return true;
    }

    IEnumerator FireAction()
    {
        transform.Rotate(new Vector3(-11f, 0f, 0f), Space.Self);
        yield return new WaitForSeconds(0.07f);

        transform.Rotate(new Vector3(11f, 0f, 0f), Space.Self);
    }
}
