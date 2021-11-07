using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleThrow : MonoBehaviour
{
    ParticleSystem particle;
    public AudioSource throwSound, throwSoundStart;
    public float throwSoundVolume = 1, fadeInThrowSound = 1, fadeOutThrowSound = 1;

    public float damage = 1f;
    bool throwing = false;

    private void OnDisable()
    {
        StopAllCoroutines();
        throwing = false;
        throwSound.volume = 0;
    }


    private void Awake()
    {
        throwSound.volume = 0;
        particle = GetComponent<ParticleSystem>();
    }


    private void OnParticleCollision(GameObject other)
    {
        Tool.SearchComponent<Enemy>(other.transform)?.TakeDamage(damage);
    }


    public void Throw(bool b)
    {
        if (throwing == b)
            return;

        throwing = b;
        if (throwing)
        {
            particle.Play();
            StartCoroutine("FadeInThrowSound");
            if (throwSoundStart != null) throwSoundStart.Play();
        }
        else {
            particle.Stop();
            StartCoroutine("FadeOutThrowSound");
        }
    }





    IEnumerator FadeInThrowSound()
    {
        StopCoroutine("FadeOutThrowSound");
        if (!throwSound.isPlaying) throwSound.Play();
        float tlast = Time.time, tnew, dt;

        while (throwSound.volume < throwSoundVolume)
        {
            tnew = Time.time;
            dt = tnew - tlast;
            tlast = tnew;

            throwSound.volume += throwSoundVolume * dt / fadeInThrowSound;

            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator FadeOutThrowSound()
    {
        StopCoroutine("FadeInThrowSound");
        float tlast = Time.time, tnew, dt;

        while (throwSound.volume > 0)
        {
            tnew = Time.time;
            dt = tnew - tlast;
            tlast = tnew;

            throwSound.volume -= throwSoundVolume * dt / fadeOutThrowSound;

            yield return new WaitForEndOfFrame();
        }

        throwSound.Stop();
    }

}
