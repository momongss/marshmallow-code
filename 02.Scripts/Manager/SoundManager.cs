using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager I { get; private set; }

    public AudioSource CoinSound;
    public AudioSource ExplosionSmall;

    public AudioSource[] CuteHits;

    [Header("PlayScene")]

    public AudioSource Grab01;
    public AudioSource Grab02;
    public AudioSource Merge01;
    public AudioSource Merge02;
    public AudioSource BuyPlayer01;
    public AudioSource BuyPlayer02;
    public AudioSource Explosion01;
    public AudioSource Explosion02;
    public AudioSource Explosion03;

    public AudioSource Gun01;
    public AudioSource Gun02;

    private void Awake()
    {
        if (I != null)
        {
            Destroy(gameObject);
            return;
        }

        I = this;

        transform.parent = null;
        DontDestroyOnLoad(gameObject);
    }

    int maxCoinSound = 20;
    int currCoinSound = 0;

    public void PlayCoin()
    {
        if (currCoinSound >= maxCoinSound) return;
        ++currCoinSound;

        StartCoroutine(playCoin());
    }

    IEnumerator playCoin()
    {
        CoinSound.PlayOneShot(CoinSound.clip);
        yield return new WaitForSeconds(CoinSound.clip.length);

        --currCoinSound;
    }

    int maxGunSound = 20;
    int currGunSound = 0;

    float prevFireTime = 0f;
    float minFireInterval = 0.01f;

    public void PlayGun(AudioSource source)
    {
        if (currGunSound > maxGunSound) return;
        float currTime = Time.time;
        if (currTime - prevFireTime < minFireInterval) return;
        prevFireTime = currTime;

        ++currGunSound;

        StartCoroutine(playGun(source));
    }

    IEnumerator playGun(AudioSource source)
    {
        source.PlayOneShot(source.clip);
        yield return new WaitForSeconds(source.clip.length);

        --currGunSound;
    }

    public void PlayRandom(AudioSource[] sources)
    {
        AudioSource selectedSource = sources[Random.Range(0, sources.Length)];
        selectedSource.PlayOneShot(selectedSource.clip);
    }
}
