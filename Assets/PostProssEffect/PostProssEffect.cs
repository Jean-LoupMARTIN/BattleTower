using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public abstract class PostProssEffect : MonoBehaviour
{

    static PostProssEffect crtEffect;
    public float t = 5;
    protected Volume postProcess;

    protected virtual void Awake()
    {
        postProcess = FindObjectOfType<Volume>();
    }


    protected virtual void OnEnable()
    {
        if (crtEffect)
        {
            crtEffect.StopCoroutine("ApplyCoroutine");
            crtEffect.Apply(1);
        }

        StartApply();
    }

    private void OnDisable()
    {
        if (crtEffect == this)
        {
            StopCoroutine("ApplyCoroutine");
            Apply(1);
            crtEffect = null;
        }
    }



    IEnumerator ApplyCoroutine()
    {
        crtEffect = this;
        float tcrt = 0;
        while (tcrt < t)
        {
            tcrt += Time.deltaTime / Time.timeScale;
            Apply(Tool.Progress(tcrt, t));
            yield return new WaitForEndOfFrame();
        }

        Apply(1);
        crtEffect = null;
    }

    protected virtual void StartApply()
    {
        StartCoroutine("ApplyCoroutine");
    }

    protected abstract void Apply(float progress);
}
