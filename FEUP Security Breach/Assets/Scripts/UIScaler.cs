using UnityEngine;

public class UIScaler : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        float f = Screen.height/768f;
        GetComponent<RectTransform>().localScale = new Vector3(f,f,f);
        Destroy(this);
    }
}
