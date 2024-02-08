using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectFeedback : MonoBehaviour
{
    RectTransform rect;
    [SerializeField] Image image;
    float time = 1.0f;
    bool add = true;

    Vector2 start;
    Vector2 end;

    // Start is called before the first frame update
    void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    public void Init(RawMaterial material, bool add)
    {
        image.sprite = material.icon;
        this.add = add;
        start = add ? Vector2.zero : Vector2.left * 50 + Vector2.up * 100;
        end = add ? Vector2.up * 100 : Vector2.left * 50;
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        rect.anchoredPosition = Vector2.Lerp(end, start, time);
        if (time < 0) Destroy(gameObject);
        foreach(var image in GetComponentsInChildren<Image>())
        {
            image.color = new Color(1, 1, 1, time);
        }
    }
}
