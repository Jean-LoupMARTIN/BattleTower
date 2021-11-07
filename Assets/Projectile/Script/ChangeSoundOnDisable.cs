using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChangeSoundOnDisable : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip[] sounds;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnDisable()
    {
        audioSource.clip = Tool.Rand(sounds);
    }
}
