using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Coin : MonoBehaviour
{
    float point;
    public GameObject coinModel;

    public Rigidbody rigid;

    public CoinPool myCoinPool;

    Transform destination;

    [SerializeField] ParticleSystem PS_sparks;

    bool isReady = false;

    public void Spawn()
    {
        rigid.useGravity = true;
        rigid.constraints = RigidbodyConstraints.None;

        rigid.velocity = Vector3.zero;

        Vector3 dir = Random.rotation.eulerAngles.normalized;
        dir.z *= 4f;

        float power = Random.Range(1f, 2f);

        rigid.AddForce(dir * power, ForceMode.Impulse);

        Player player = PlayerSquad.I.GetRandomPlayer();

        if (player != null)
        {
            destination = player.transform;
            StartCoroutine(_Spawn());
        }
    }

    IEnumerator _Spawn()
    {
        rigid.useGravity = true;
        isReady = false;
        yield return new WaitForSeconds(1.5f);

        rigid.useGravity = false;
        isReady = true;
    }

    private void FixedUpdate()
    {
        if (isReady && destination != null)
        {
            Vector3 movePosition = Vector3.MoveTowards(transform.position, destination.position, Time.fixedDeltaTime * 8f);

            rigid.MovePosition(movePosition);
        }
    }

    public void SetPoint(float _point)
    {
        point = _point;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Layer.PLAYER)
        {
            SoundManager.I.PlayCoin();
            BattleSceneManager.I.SetPoint(BattleSceneManager.I.point + (int)Mathf.Ceil(Take()));
        }
    }

    public float Take()
    {
        ParticleSystem _ps = (ParticleSystem)PoolGeneral.I._Instantiate(PS_sparks, transform.position, Quaternion.identity);
        _ps.Play();
        PoolGeneral.I._Destroy(_ps, 1.2f);

        rigid.useGravity = false;
        rigid.constraints = RigidbodyConstraints.FreezePosition;

        myCoinPool.Destroy(this);
        return point;
    }
}
