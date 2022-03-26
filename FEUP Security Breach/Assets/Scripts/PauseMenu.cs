using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;
    private bool toggler = false;
    void Awake()
    {
        instance = this;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            AudioController.instance.Play("Click");
            if (toggler)
            {
                Resume();
            }
            else
            {
                toggler = true;
                PortalGun.toggler = false;
                Time.timeScale = 0;
                transform.GetChild(0).gameObject.SetActive(true);
                GameObject.Find("EventSystem").GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
            }
        }
    }
    public void Resume()
    {
        toggler = false;
        transform.GetChild(0).gameObject.SetActive(false);
        Time.timeScale = 1;
        PortalGun.toggler = true;
    }
    public void CheckPoint()
    {
        toggler = false;
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

    public void EndGame()
    {
        if (toggler) Resume();
        enabled = false;
    }

    IEnumerator ConfirmCheckPoint()
    {
        yield return StartCoroutine(Confirmation.instance.Confirm("Are you sure you want to revert back to the last checkpoint?"));
        if (Confirmation.instance.answer)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            yield return StartCoroutine(Fader.instance.FadeOut());
            TimeCounter.instance.counting = false;
            Time.timeScale = 1;
            toggler = false;
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
            Time.timeScale = 1;
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
            Time.timeScale = 1;
            SceneManager.LoadScene(0);
        }
    }
}
