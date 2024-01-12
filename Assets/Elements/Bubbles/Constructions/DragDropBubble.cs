using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDropBubble : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RawMaterial construction;
    protected Predicate<PointerEventData> condition;

    protected RectTransform m_RectTransform;
    [SerializeField] protected Image m_Image;

    protected bool gotTheRawMats = false;
    protected Storage lumStorage;

    protected virtual void Awake()
    {
        condition = CanBeBuild;
        m_RectTransform = GetComponent<RectTransform>();
        lumStorage = GameManager.instance.lumberjack.storage;
    }

    private void OnEnable()
    {
        UpdateGotMats();
    }


    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        m_Image.color = gotTheRawMats ? Color.white : new Color(1, 0, 0, .4f);
        if (!gotTheRawMats )
        {
            Debug.Log("Bulle Feedback");
            return;
        }
        foreach (var dd in transform.parent.GetComponentsInChildren<DragDropBubble>())
        {
            if (dd == this) continue;
            dd.gameObject.SetActive(false);
        }
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
        if (Vector2.Distance(eventData.position, eventData.pressPosition) < 100.0f)
        {
            Debug.Log("Feedback DND");
        }
        if (!gotTheRawMats) return;
        if (condition.Invoke(eventData)) OnSuccess(eventData);
        else OnError(eventData); 
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            transform.parent.GetChild(i).gameObject.SetActive(true);
        }
    }
    protected virtual void OnSuccess(PointerEventData eventData)
    {
        m_Image.color = new Color(1, 1, 1, 1);
        m_RectTransform.anchoredPosition = Vector3.zero;
        lumStorage.Add(construction, -1);
        UpdateAllGotMats();
    }
    protected virtual void OnError(PointerEventData eventData)
    {
        m_Image.color = new Color(1, 1, 1, .4f);
        m_RectTransform.anchoredPosition = Vector3.zero;
    }


    #region Setup
    protected virtual bool CanBeBuild(PointerEventData eventData)
    {
        return gotTheRawMats;
    }
    public void UpdateGotMats()
    {
        if (lumStorage == null)
        {
            lumStorage = GameManager.instance.lumberjack.storage;
            if (lumStorage == null)
                lumStorage = FindObjectOfType<Lumberjack>().storage;
        }

        if (lumStorage.Count(construction) > 0)
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
        foreach (var bubble in GameManager.instance.lumberjack.plans.GetComponentsInChildren<DragDropBubble>())
        {
            bubble.UpdateGotMats();
        }
    }
    #endregion
}
