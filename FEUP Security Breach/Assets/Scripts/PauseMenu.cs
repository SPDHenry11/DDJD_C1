using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;
    void Awake()
    {
        instance=this;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }
    public void Resume()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        Time.timeScale = 1;
    }
    public void CheckPoint()
    {
        StartCoroutine(ConfirmCheckPoint());
    }
    public void Restart()
    {
        StartCoroutine(ConfirmRestart());
    }
    public void MainMenu()
    {
        StartCoroutine(ConfirmMainMenu());
    }
    public void Quit()
    {
        StartCoroutine(ConfirmQuit());
    }

    IEnumerator ConfirmCheckPoint()
    {
        yield return StartCoroutine(Confirmation.instance.Confirm("Are you sure you want to revert back to the last checkpoint?"));
        if (Confirmation.instance.answer)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            Time.timeScale = 1;
            yield return StartCoroutine(Fader.instance.FadeOut());
            yield return new WaitForSeconds(1);
            GameController.instance.checkPoint.Restart();
        }
        StartCoroutine(Fader.instance.FadeIn());
    }

    IEnumerator ConfirmRestart()
    {
        yield return StartCoroutine(Confirmation.instance.Confirm("Are you sure you want to restart the whole level?"));
        if (Confirmation.instance.answer)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    IEnumerator ConfirmQuit()
    {
        yield return StartCoroutine(Confirmation.instance.Confirm("Are you sure you want to quit the game?"));
        if (Confirmation.instance.answer)
        {
            Application.Quit();
        }
    }
    IEnumerator ConfirmMainMenu()
    {
        yield return StartCoroutine(Confirmation.instance.Confirm("Are you sure you want go back to the main menu?"));
        if (Confirmation.instance.answer)
        {
            SceneManager.LoadScene(0);
        }
    }
}
