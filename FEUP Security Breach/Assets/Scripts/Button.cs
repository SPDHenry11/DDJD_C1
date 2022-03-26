using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        AudioController.instance.Play("Click");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponentInChildren<Text>().color = Color.white;
        AudioController.instance.Play("MouseEnter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponentInChildren<Text>().color = new Color(0.8f, 0.8f, 0.8f, 1);
    }


}
