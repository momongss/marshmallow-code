using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] audioClips;

    private void Reset()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void PlayRandom()
    {
        AudioClip selectedClip = audioClips[Random.Range(0, audioClips.Length)];
        audioSource.PlayOneShot(selectedClip);
    }
}
