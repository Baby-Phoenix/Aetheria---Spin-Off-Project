using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleFade : MonoBehaviour
{
    public List<ParticleSystem> particleSystems;
    public float fadeInDuration = 1f;
    public float fadeOutDuration = 1f;
    public float delayTime = 2f;

    public bool isFadingIn = false;
    public bool isFadingOut = false;
    private float alpha = 0f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            FadeInAndOut();
        }
    }

    public bool getIsFadeOut()
    {
        return isFadingOut;
    }

    public bool getIsFadeIn()
    {
        return isFadingIn;
    }

    public void FadeIn()
    {
        if (!isFadingIn && !isFadingOut)
        {
            StartCoroutine(FadeInCoroutine());
        }
    }

    public void FadeOut()
    {
        if (!isFadingIn && !isFadingOut)
        {
            StartCoroutine(FadeOutCoroutine());
        }
    }

    private IEnumerator FadeInCoroutine()
    {
        isFadingIn = true;
        alpha = 0f;
        while (alpha < 1f)
        {
            alpha += Time.deltaTime / fadeInDuration;
            SetAlpha(alpha);
            yield return null;
        }

        isFadingIn = false;
    }

    private IEnumerator FadeOutCoroutine()
    {
        isFadingOut = true;
        alpha = 1f;
        while (alpha > 0f)
        {
            alpha -= Time.deltaTime / fadeOutDuration;
            SetAlpha(alpha);
            yield return null;
        }
        isFadingOut = false;
    }

    public void FadeInAndOut()
    {
        if (!isFadingIn && !isFadingOut)
        {
            StartCoroutine(FadeInAndOutCoroutine());
        }
    }

    private IEnumerator FadeInAndOutCoroutine()
    {
        isFadingIn = true;
        alpha = 0f;
        while (alpha < 1f)
        {
            alpha += Time.deltaTime / fadeInDuration;
            SetAlpha(alpha);
            yield return null;
        }

        yield return new WaitForSeconds(delayTime);

        isFadingOut = true;
        alpha = 1f;
        while (alpha > 0f)
        {
            alpha -= Time.deltaTime / fadeOutDuration;
            SetAlpha(alpha);
            yield return null;
        }
        isFadingIn = false;
        isFadingOut = false;
    }

    private void SetAlpha(float alpha)
    {
        foreach (var ps in particleSystems)
        {
            var main = ps.main;
            var color = main.startColor.color;
            color.a = alpha;
            main.startColor = color;
        }
    }
}
