using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance => _instance;
    
    [SerializeField] private AudioSource _sfx;

    private void Awake()
    {
        _instance = this;
    }

    public void PlayOneShot(AudioClip audioClip, float vol = 1)
    {
        _sfx.PlayOneShot(audioClip, vol);
    }
}
