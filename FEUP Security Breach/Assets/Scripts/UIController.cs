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

    IEnumerator FadeIn()
    {
        while (fader.color.a > 0)
        {
            fader.color = new Color(0, 0, 0, fader.color.a - 0.5f * Time.deltaTime);
            yield return null;
        }
        fader.color = new Color(0, 0, 0, 0);
    }
    IEnumerator FadeOut()
    {
        while (fader.color.a < 1)
        {
            fader.color = new Color(0, 0, 0, fader.color.a + 0.5f * Time.deltaTime);
            yield return null;
        }
        fader.color = new Color(0, 0, 0, 1);
    }
}
