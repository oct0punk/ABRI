using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDropBubble : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    protected Predicate<PointerEventData> condition;
    protected RectTransform m_RectTransform;
    [SerializeField] protected Image m_Image;
    public CraftMaterials[] CraftMaterials;
    protected static Storage lumStorage;
    public bool gotTheRawMats = false;

    protected virtual void Awake()
    {
        condition = CanBeBuild;
        m_RectTransform = GetComponent<RectTransform>();
        lumStorage = FindObjectOfType<Lumberjack>().storage;
    }

    private void Start()
    {
        if (!Tuto.buildDone)
        {
            m_Image.color = Color.yellow;
        }
    }


    

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if (!gotTheRawMats) return;
        m_Image.color = Color.white;
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
        m_Image.color = new Color(1, 1, 1, .4f);
        m_RectTransform.anchoredPosition = Vector3.zero;
        Tuto.buildDone = true;
        lumStorage.Consume(CraftMaterials);
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
        gotTheRawMats = lumStorage.CanCraft(CraftMaterials);
    }
}
