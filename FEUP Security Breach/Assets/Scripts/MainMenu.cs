using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Text highScore;
    void Awake()
    {
        float score = SaveLoadStats.Load();
        if(score<0) highScore.text = "HighScore: ----";
        else highScore.text = "HighScore: " + score.ToString("F1") + 's';
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
