using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scene_Manager: MonoBehaviour
{
    public Image[] titleImages;
    public float fadeInDuration = 0.5f;
    public float displayDuration = 1.0f;
    public float fadeOutDuration = 0.5f;

   // public GameObject[] text;
    private int i = 0;

    private void Start()
    {
        //text[0].SetActive(false);
        //text[1].SetActive(false);
        StartCoroutine(SequenceManager());
    }

    IEnumerator SequenceManager()
    {
        foreach (var image in titleImages)
        {
            yield return StartCoroutine(FadeImage(image, 1f, fadeInDuration));
            if (i > 0)
            {
               // text[i].SetActive(true);
               // text[i-1].SetActive(false);
            }
            i++;

            yield return new WaitForSeconds(displayDuration);

            yield return StartCoroutine(FadeImage(image, 0f, fadeOutDuration));
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    IEnumerator FadeImage(Image image, float targetAlpha, float duration)
    {
        float startAlpha = image.color.a;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            image.color = new Color(image.color.r, image.color.g, image.color.b, newAlpha);
            yield return null;
        }

        image.color = new Color(image.color.r, image.color.g, image.color.b, targetAlpha);
    }
}
