using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    [SerializeField] private TextMeshProUGUI textCoins;
    private int coins = 0;
    [SerializeField] private Image fader;
    [SerializeField] private GameObject busted;
    void Awake()
    {
        instance = this;
        fader.color = new Color(0, 0, 0, 1);
    }
    void Start()
    {
        StartCoroutine(FadeIn());
    }

    public void AddCoin()
    {
        coins++;
        textCoins.text = "Coins " + coins.ToString();
    }

    public bool PurchaseCoffee()
    {
        if (coins >= 5)
        {
            coins -= 5;
            textCoins.text = "Coins " + coins.ToString();
            return true;
        }
        return false;
    }

    public IEnumerator FadeIn()
    {
        while (fader.color.a > 0)
        {
            fader.color = new Color(0, 0, 0, fader.color.a - 0.5f * Time.deltaTime);
            yield return null;
        }
        fader.color = new Color(0, 0, 0, 0);
    }
    public IEnumerator FadeOut()
    {
        while (fader.color.a < 1)
        {
            fader.color = new Color(0, 0, 0, fader.color.a + 0.5f * Time.deltaTime);
            yield return null;
        }
        fader.color = new Color(0, 0, 0, 1);
    }
    public void Busted()
    {
        if (GameController.imunity) return;
        GameController.imunity = true;
        StopAllCoroutines();
        StartCoroutine(BustedEffect());
    }
    IEnumerator BustedEffect()
    {
        Movement.instance.enabled = false;
        busted.SetActive(true);
        RectTransform rect = busted.GetComponent<RectTransform>();
        TextMeshProUGUI text = busted.GetComponent<TextMeshProUGUI>();
        rect.localScale = new Vector3(5, 5, 5);
        float scale = 5;
        while (scale > 1)
        {
            scale -= 10 * Time.deltaTime;
            rect.localScale = new Vector3(scale, scale, scale);
            text.faceColor = new Color32(255, 255, 255, (byte)(text.faceColor.a + 510 * Time.deltaTime));
            yield return null;
        }
        text.faceColor = new Color32(255, 255, 255, 255);
        rect.localScale = new Vector3(1, 1, 1);
        yield return StartCoroutine(FadeOut());
        yield return new WaitForSeconds(2);
        while (text.faceColor.a > 0)
        {
            text.faceColor = new Color32(255, 255, 255, (byte)(text.faceColor.a - 255 * Time.deltaTime));
            yield return null;
        }
        text.faceColor = new Color32(255, 255, 255, 0);
        busted.SetActive(false);
        GameController.instance.checkPoint.Restart();
        Movement.instance.enabled = true;
        yield return StartCoroutine(FadeIn());
        yield return new WaitForSeconds(1);
        GameController.imunity = false;
    }
}
