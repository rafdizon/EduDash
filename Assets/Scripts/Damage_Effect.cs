using System.Collections;
using UnityEngine;

public class Damage_Effect : MonoBehaviour
{
    public float fadeDuration = 0.5f; 
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Color color = spriteRenderer.color;
        color.a = 0f;
        spriteRenderer.color = color;

        gameObject.SetActive(false); 
    }

    public void TriggerDamageEffect()
    {
        gameObject.SetActive(true); 
        StartCoroutine(FadeOverlay());
    }

    private IEnumerator FadeOverlay()
    {
        // Fade in
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            Color color = spriteRenderer.color;
            color.a = Mathf.Lerp(0, 0.5f, t / fadeDuration); 
            spriteRenderer.color = color;
            yield return null;
        }

        // Fade out
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            Color color = spriteRenderer.color;
            color.a = Mathf.Lerp(0.5f, 0, t / fadeDuration); 
            spriteRenderer.color = color;
            yield return null;
        }

        gameObject.SetActive(false); 
    }
}
