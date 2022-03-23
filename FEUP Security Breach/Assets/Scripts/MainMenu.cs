using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI highScore;
    void Awake()
    {
        highScore.text = "HighScore: " + SaveLoadStats.Load();
    }
    public void LoadLevel()
    {
        //play audio
        SceneManager.LoadScene(1);
    }
    public void Quit()
    {
        StartCoroutine(QuitConfirmation());
    }
    IEnumerator QuitConfirmation()
    {

        yield return StartCoroutine(Confirmation.instance.Confirm("Are you sure you want to Quit?"));
        if (Confirmation.instance.answer) Application.Quit();
    }
}
