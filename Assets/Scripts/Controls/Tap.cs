using UnityEngine;
using UnityEngine.EventSystems;

public class Tap : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    bool pressed;

    private void Update()
    {
        if (pressed)
        Debug.Log(pressed, this);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pressed = true;
        SwipeManager.buttonPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pressed = false;
        SwipeManager.buttonPressed = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (pressed)
        SwipeManager.buttonPressed = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (pressed)
        SwipeManager.buttonPressed = false;
    }

}
