using UnityEngine;
using System.Collections;


/// <Summary>
/// Using a Transparent Material, it fades in and out a 3d object.
/// Changes: Uses objectRenderer, not spriteRenderer.
/// </Summary>
public class FadeInFadeOut : MonoBehaviour
{
    public float fadeDuration = 2.0f;
    private float currentAlpha = 0.0f;
    private bool isFadingIn = true;

    private Renderer objectRenderer;

    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        Color color = objectRenderer.material.color;

        float startAlpha = isFadingIn ? 0f : 1f;
        float endAlpha   = isFadingIn ? 1f : 0f;

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            currentAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);

            color.a = currentAlpha;
            objectRenderer.material.color = color;

            yield return null;
        }
        Debug.Log("Working");
        color.a = endAlpha;
        objectRenderer.material.color = color;

        isFadingIn = !isFadingIn;
        StartCoroutine(Fade());
    }
}
