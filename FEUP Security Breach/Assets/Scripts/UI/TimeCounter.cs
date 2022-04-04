using UnityEngine;
using TMPro;

/// <summary>
/// Manages the highscore time value
/// </summary>
public class TimeCounter : MonoBehaviour
{
    private TextMeshProUGUI text;
    public static TimeCounter instance;
    [HideInInspector] public bool counting = false;
    [HideInInspector] public static float time = 0;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        instance = this;
        time = 0;
    }

    void Update()
    {
        if (counting)
        {
            time += Time.deltaTime;
            text.text = (Mathf.Round(10 * time) / 10).ToString("F1") + 's';
        }
    }
    public void EndGame()
    {
        counting = false;
        if(SaveLoadStats.Save()) Notifications.instance.Notify("New Record!");
    }
}
