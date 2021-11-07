using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BigBoom : PostProssEffect
{
    Dictionary<AudioSource, float> sourcesModified = new Dictionary<AudioSource, float>();


    public AnimationCurve timeSpeedCurve;
    WhiteBalance whiteBalance;
    Vignette vignette;
    public Color vignetteColor;
    public float vignetteIntensity;
    float vignetteIntensityMem;


    protected override void Awake()
    {
        base.Awake();
        postProcess.profile.TryGet(out whiteBalance);
        postProcess.profile.TryGet(out vignette);
    }


    protected override void StartApply()
    {
        vignetteIntensityMem = vignette.intensity.value;
        base.StartApply();
    }


    protected override void Apply(float progress)
    {
        Time.timeScale = timeSpeedCurve.Evaluate(progress);


        List<AudioSource> sourcesActive = FindObjectsOfType<AudioSource>().ToList();

        foreach (AudioSource source in sourcesActive)
            if (!sourcesModified.ContainsKey(source))
                sourcesModified.Add(source, source.pitch);

        List<AudioSource> sourceToRemove = new List<AudioSource>();
        foreach (KeyValuePair<AudioSource, float> source in sourcesModified)
            if (!sourcesActive.Contains(source.Key))
                sourceToRemove.Add(source.Key);

        foreach (AudioSource source in sourceToRemove)
        {
            if (source) source.pitch = sourcesModified[source];
            sourcesModified.Remove(source);
        }


        foreach (KeyValuePair<AudioSource, float> source in sourcesModified)
        source.Key.pitch = Time.timeScale * source.Value;

        whiteBalance.temperature.value = 100 * (1 - progress);
        vignette.intensity.value = Mathf.Lerp(vignetteIntensity, vignetteIntensityMem, progress);
        vignette.color.value = Color.Lerp(vignetteColor, Color.black, progress);
    }

}
