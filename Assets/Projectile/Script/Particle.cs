using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem), typeof(AudioSource))]
public class Particle : MonoBehaviour
{
    [Range(0, 3)]
    public float pitch = 1;
    public float dpitch = 0.15f;

    ParticleSystem particle;
    AudioSource sound;
    public float shake = 0;



    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
        sound = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        if (shake > 0) Tool.ShakeSphere(transform.position, shake);
    }

    private void OnDisable()
    {
        sound.pitch = pitch + Random.Range(-dpitch, dpitch);
    }

    private void Update()
    {
        if (particle.isStopped && !sound.isPlaying)
            QueueManager.Desactive(gameObject);
    }
}
