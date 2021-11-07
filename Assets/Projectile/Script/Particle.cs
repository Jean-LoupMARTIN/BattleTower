using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem), typeof(AudioSource))]
public class Particle : MonoBehaviour
{
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

    private void Update()
    {
        if (particle.isStopped && !sound.isPlaying)
            QueueManager.Desactive(gameObject);
    }
}
