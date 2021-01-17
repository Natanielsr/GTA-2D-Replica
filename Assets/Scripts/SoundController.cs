using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public AudioClip[] audioClips;
    public AudioSource audioSource;

    public AudioSource gunAudioSource;
    public AudioClip gunCockClip;
    public AudioClip gunShotClip;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playRndAudioClip()
    {
        int rnd = Random.Range(0, audioClips.Length);

        Debug.Log(rnd);
        audioSource.clip = audioClips[rnd];
        audioSource.Play();
    }

    public void playGetGun()
    {
        gunAudioSource.clip = gunCockClip;
        gunAudioSource.Play();
    }

    public void playShot()
    {
        gunAudioSource.clip = gunShotClip;
        gunAudioSource.Play();
    }
}
