using System;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDropBubble : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    protected Predicate<PointerEventData> condition;
    protected RectTransform m_RectTransform;
    [SerializeField] protected Image m_Image;
    public Construction construction;
    protected bool gotTheRawMats = false;
    protected Storage lumStorage;

    protected virtual void Awake()
    {
        condition = CanBeBuild;
        m_RectTransform = GetComponent<RectTransform>();
        lumStorage = FindObjectOfType<Lumberjack>().storage;
    }

    

    public virtual void OnBeginDrag(PointerEventData eventData)
    {   
        m_Image.color = gotTheRawMats ? Color.white : new Color(1, 0, 0, .4f);
    }


    public virtual void OnDrag(PointerEventData eventData)
    {
        if (!gotTheRawMats) return;
        m_RectTransform.anchoredPosition += eventData.delta / GetComponentInParent<Canvas>().scaleFactor;
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
        if (!gotTheRawMats) return;
        if (condition.Invoke(eventData)) OnSuccess(eventData);
        else OnError(eventData);
    }
    protected virtual void OnSuccess(PointerEventData eventData)
    {
        m_Image.color = new Color(1, 1, 1, 1);
        m_RectTransform.anchoredPosition = Vector3.zero;
        lumStorage.Consume(construction.materials);
        UpdateAllGotMats();
    }
    protected virtual void OnError(PointerEventData eventData)
    {
        m_Image.color = new Color(1, 1, 1, .4f);
        m_RectTransform.anchoredPosition = Vector3.zero;
    }
    
    
    protected virtual bool CanBeBuild(PointerEventData eventData)
    {
        return gotTheRawMats;
    }
    private void OnEnable()
    {
        UpdateGotMats();
    }
    public void UpdateGotMats()
    {
        if (lumStorage == null)
        {
            lumStorage = FindObjectOfType<Lumberjack>().storage;
            if (lumStorage == null)
                return;
        }
        if (lumStorage.CanCraft(construction.materials))        
            Enable();        

        else
            Block();        
    }
    public void Enable()
    {
        gotTheRawMats = true;
        m_Image.color = new Color(1, 1, 1, .4f);
    }
    public virtual void Block()
    {
        gotTheRawMats = false;
        m_Image.color = new Color(1, 0, 0, .4f);
    }
    public static void UpdateAllGotMats()
    {
        foreach (var bubble in GameManager.instance.lumberjack.constructUI.GetComponentsInChildren<DragDropBubble>())
        {
            bubble.UpdateGotMats();
        }
    }
}
