using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages screen fade in and fade out
/// </summary>
public class Fader : MonoBehaviour
{
    public static Fader instance;
    bool tracker = false;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        StartCoroutine(FadeIn());
    }

    public IEnumerator FadeIn()
    {
        Image fader = transform.GetChild(0).GetComponent<Image>();
        fader.color = new Color(0, 0, 0, 1);
        tracker = true;
        while (fader.color.a > 0)
        {
            if (!tracker) yield break;
            fader.color = new Color(0, 0, 0, fader.color.a - 0.5f * Time.unscaledDeltaTime);
            yield return null;
        }
        fader.color = new Color(0, 0, 0, 0);
        if(TimeCounter.instance!=null) TimeCounter.instance.counting = true;
        transform.GetChild(0).gameObject.SetActive(false);
    }
    public IEnumerator FadeOut()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        Image fader = transform.GetChild(0).GetComponent<Image>();
        tracker = false;
        while (fader.color.a < 1)
        {
            if (tracker) yield break;
            fader.color = new Color(0, 0, 0, fader.color.a + 0.5f * Time.unscaledDeltaTime);
            yield return null;
        }
        fader.color = new Color(0, 0, 0, 1);
    }
}
