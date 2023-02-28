using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class BattleCamera : MonoBehaviour
{
    public static BattleCamera I { get; private set; }

    [SerializeField] CinemachineVirtualCamera vcam;

    CinemachineBasicMultiChannelPerlin perlin;

    Coroutine shakeRoutine = null;

    private void Awake()
    {
        I = this;

        perlin = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        perlin.m_AmplitudeGain = 0f;
        perlin.m_FrequencyGain = 0f;
    }

    private void Reset()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
    }

    public void Shake(float amount, float freq, float duration = 0.2f)
    {
        perlin.m_AmplitudeGain = amount;
        perlin.m_FrequencyGain = freq;

        if (shakeRoutine != null)
        {
            StopCoroutine(shakeRoutine);
        }

        shakeRoutine = StartCoroutine(StopShake(duration));
    }   

    IEnumerator StopShake(float duration)
    {
        yield return new WaitForSeconds(duration);

        perlin.m_AmplitudeGain = 0f;
        perlin.m_FrequencyGain = 0f;

        shakeRoutine = null;
    }
}
