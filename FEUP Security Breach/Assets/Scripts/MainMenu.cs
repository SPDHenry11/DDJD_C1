using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
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
        if(Confirmation.instance.answer) Application.Quit();
    }
}
