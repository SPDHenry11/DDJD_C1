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
    }

    public IEnumerator Confirm(string text)
    {
        answer = false;
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(0).GetChild(0).GetComponent<Text>().text = text;
        done = false;
        while (!done)
        {
            yield return null;
        }
        yield return null;
        transform.GetChild(0).gameObject.SetActive(false);
    }
}