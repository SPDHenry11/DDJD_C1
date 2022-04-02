using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages overall UI and UI related functions
/// </summary>
public class UIController : MonoBehaviour
{
    public static UIController instance;
    [SerializeField] private TextMeshProUGUI textCoins;
    private int coins = 0;
    [SerializeField] private GameObject busted;
    [SerializeField] private GameObject endGame;
    void Awake()
    {
        instance = this;
    }

    public void AddCoin()
    {
        coins++;
        textCoins.text = "x" + coins.ToString();
    }

    public bool PurchaseCoffee()
    {
        if (coins >= 5)
        {
            coins -= 5;
            textCoins.text = "x" + coins.ToString();
            return true;
        }
        return false;
    }
    public void Busted()
    {
        if (GameController.imunity) return;
        GameController.imunity = true;
        StartCoroutine(BustedEffect());
        Movement.instance.GetComponent<Rigidbody2D>().drag = 2;
    }

    public void End()
    {
        GameController.imunity = true;
        StartCoroutine(EndGame());
    }
    IEnumerator BustedEffect()
    {
        TimeCounter.instance.counting = false;
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
            text.faceColor = new Color32(255, 255, 255, (byte)Mathf.Min(text.faceColor.a + 510 * Time.deltaTime, 255));
            yield return null;
        }
        AudioController.instance.Play("Bust");
        StartCoroutine(AudioController.instance.FadeOut());
        text.faceColor = new Color32(255, 255, 255, 255);
        rect.localScale = new Vector3(1, 1, 1);
        yield return StartCoroutine(Fader.instance.FadeOut());
        yield return new WaitForSeconds(1);
        while (text.faceColor.a > 0)
        {
            text.faceColor = new Color32(255, 255, 255, (byte)Mathf.Max(text.faceColor.a - 255 * Time.deltaTime, 0));
            yield return null;
        }
        text.faceColor = new Color32(255, 255, 255, 0);
        busted.SetActive(false);
        GameController.instance.checkPoint.Restart();
        Movement.instance.enabled = true;
        Movement.instance.GetComponent<Rigidbody2D>().drag = 0.5f;
        StartCoroutine(AudioController.instance.FadeIn());
        yield return StartCoroutine(Fader.instance.FadeIn());
        yield return new WaitForSeconds(1);
        GameController.imunity = false;
    }

    IEnumerator EndGame()
    {
        TimeCounter.instance.EndGame();
        AudioController.instance.Stop("Music");
        AudioController.instance.Play("Win");
        endGame.transform.GetChild(1).GetComponent<Text>().text = "Score: " + TimeCounter.time.ToString("F1") + 's';
        CanvasGroup cg = endGame.GetComponent<CanvasGroup>();
        cg.alpha = 0;
        yield return StartCoroutine(Fader.instance.FadeOut());
        endGame.SetActive(true);
        while (cg.alpha < 1)
        {
            cg.alpha += Time.unscaledDeltaTime;
            yield return null;
        }
        cg.alpha = 1;
        yield return new WaitForSeconds(2);
        StartCoroutine(AudioController.instance.FadeOut());
        yield return new WaitForSeconds(1);
        while (cg.alpha > 0)
        {
            cg.alpha -= Time.unscaledDeltaTime;
            yield return null;
        }
        SceneManager.LoadScene(0);
    }
}
