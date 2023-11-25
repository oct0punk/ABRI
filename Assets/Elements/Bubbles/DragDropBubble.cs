using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDropBubble : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    protected Predicate<PointerEventData> condition;
    protected RectTransform m_RectTransform;
    public Image m_Image;

    protected void Awake()
    {
        m_RectTransform = GetComponent<RectTransform>();
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        m_Image.color = Color.white;
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        Debug.Log(m_RectTransform);
        m_RectTransform.anchoredPosition += eventData.delta;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        if (condition.Invoke(eventData)) OnSuccess(eventData);
        else OnError(eventData);
    }

    protected virtual void OnSuccess(PointerEventData eventData)
    {
        Debug.Log("OnSuccess");
        m_Image.color = new Color(1, 1, 1, .4f);
        m_RectTransform.anchoredPosition = Vector3.zero;
    }

    protected virtual void OnError(PointerEventData eventData)
    {
        Debug.Log("OnError");
        m_Image.color = new Color(1, 1, 1, .4f);
        m_RectTransform.anchoredPosition = Vector3.zero;
    }
}
