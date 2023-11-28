using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDropBubble : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
    protected Predicate<PointerEventData> condition;
    protected RectTransform m_RectTransform;
    public Image m_Image;

    protected virtual void Awake()
    {
        condition = CanBeBuild;
        m_RectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        if (!Tuto.buildDone)
        {
            m_Image.color = Color.yellow;
        }
    }


    public virtual void OnPointerDown(PointerEventData eventData)
    {

    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        m_Image.color = Color.white;
    }


    public virtual void OnDrag(PointerEventData eventData)
    {
        m_RectTransform.anchoredPosition += eventData.delta;
        if (condition.Invoke(eventData))
            OnDragCorrect(eventData);
        else
            OnDragUncorrect(eventData);
    }

    public virtual void OnDragCorrect(PointerEventData eventData)
    {
        m_Image.color = Color.green;
    }

    public virtual void OnDragUncorrect(PointerEventData eventData)
    {
        m_Image.color = Color.red;

    }

    
    public virtual void OnEndDrag(PointerEventData eventData)
    {
        if (condition.Invoke(eventData)) OnSuccess(eventData);
        else OnError(eventData);
    }

    protected virtual void OnSuccess(PointerEventData eventData)
    {
        m_Image.color = new Color(1, 1, 1, .4f);
        m_RectTransform.anchoredPosition = Vector3.zero;
        Tuto.buildDone = true;
    }

    protected virtual void OnError(PointerEventData eventData)
    {
        m_Image.color = new Color(1, 1, 1, .4f);
        m_RectTransform.anchoredPosition = Vector3.zero;
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {

    }
    
    
    protected virtual bool CanBeBuild(PointerEventData eventData)
    {
        return false;
    }


}
