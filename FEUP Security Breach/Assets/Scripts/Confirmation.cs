using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Confirmation : MonoBehaviour
{
    public static Confirmation instance;
    [HideInInspector]
    public bool answer = false;
    private bool done = false;

    private void Awake()
    {
        instance = this;
    }

    public void setAnswer(bool a)
    {
        answer = a;
        done = true;
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public IEnumerator Confirm(string text)
    {
        answer = false;
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(0).GetChild(0).GetComponent<Text>().text = text;
        done = false;
        while (!done)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                setAnswer(false);
            }
            yield return null;
        }
    }
}