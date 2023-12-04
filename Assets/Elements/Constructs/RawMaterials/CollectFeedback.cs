using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectFeedback : MonoBehaviour
{
    RectTransform rect;
    [SerializeField] Image image;

    // Start is called before the first frame update
    void Start()
    {
        rect = GetComponent<RectTransform>();
        Destroy(gameObject, 1.0f);
    }

    public void Init(Lumberjack l, Pickable pick)
    {
        image.sprite = pick.material.icon;        
    }

    // Update is called once per frame
    void Update()
    {
        rect.position += Vector3.up * Time.deltaTime;
    }
}
