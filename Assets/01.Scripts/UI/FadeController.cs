using System.Collections;
using UnityEngine;
using UnityEngine.UI; 

public class FadeController : MonoBehaviour
{
    public Image image;
    public float fadeDuration;

    public void Start()
    {
        StartFadeIn();
    }
    public void StartFadeIn()
    {
        StartCoroutine(FadeInCoroutine());
    }

    public IEnumerator FadeInCoroutine()
    {
        Color color = image.color;
        float startA = color.a;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime; 
            float alpha = Mathf.Lerp(startA, 0f, elapsed / fadeDuration);
            image.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        image.color = new Color(color.r, color.g, color.b, 0f);
    }
}
