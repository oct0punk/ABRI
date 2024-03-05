using UnityEngine;

public class ScreenCanvas : MonoBehaviour
{
    public Transform anchor;
    RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        rectTransform.position = Camera.main.WorldToScreenPoint(anchor.position);
    }
}
