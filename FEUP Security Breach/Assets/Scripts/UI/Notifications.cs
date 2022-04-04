using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum NotificationType
{
    Checkpoint,
    SpeedBoost,
    NewRecord
}
public class Notifications : MonoBehaviour
{
    public static Notifications instance;
    private Queue<string> messages;
    private bool notifying = false;

    void Awake()
    {
        instance = this;
        messages = new Queue<string>();
    }

    public void Notify(string message)
    {
        messages.Enqueue(message);
        if (!notifying)
        {
            notifying = true;
            transform.GetChild(0).gameObject.SetActive(true);
            StartCoroutine(Notifying());
        }
    }

    IEnumerator Notifying()
    {
        TextMeshProUGUI text = GetComponentInChildren<TextMeshProUGUI>();
        text.text = messages.Dequeue();
        text.alpha = 0;
        while (text.alpha < 1)
        {
            text.alpha += Time.unscaledDeltaTime * 2;
            yield return null;
        }
        text.alpha = 1;
        yield return new WaitForSecondsRealtime(3);
        while (text.alpha > 0)
        {
            text.alpha -= Time.unscaledDeltaTime;
            yield return null;
        }
        text.alpha = 0;
        if (messages.Count > 0) StartCoroutine(Notifying());
        else {
            transform.GetChild(0).gameObject.SetActive(false);
            notifying=false;
        }
    }
}

