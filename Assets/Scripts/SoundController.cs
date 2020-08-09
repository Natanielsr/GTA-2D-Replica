﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public AudioClip[] audioClips;
    public AudioSource audioSource;
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
}
