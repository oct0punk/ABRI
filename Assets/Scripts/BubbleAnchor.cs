using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class bubbleAnchor : MonoBehaviour
{
    Lumberjack lum;
    RectTransform rect;
    public Vector2 offset = Vector2.one * .1f;
    public BubbleDot[] dots;
    public bool dotsDisplayed { get; private set; }

    // Start is called before the first frame update
    protected void Start()
    {
        lum = GameManager.instance.lumberjack;
        rect = GetComponent<RectTransform>();
        dotsDisplayed = false;
    }


    private void OnDisable()
    {
        foreach (BubbleDot dot in dots)
        {
            dot.go.gameObject.SetActive(false);
            dotsDisplayed = false;
        }
    }

    public IEnumerator DisplayDots()
    {
        for (int i = dots.Length - 1; i >= 0; i--) { 
            dots[i].go.gameObject.SetActive(true);
            yield return new WaitForSeconds(.2f);
        }
        dotsDisplayed = true;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(lum.thinkBubbleTarget.position);
        pos.x = Mathf.Clamp(pos.x, Screen.width * offset.x, Screen.width * (1 - offset.x));
        pos.y = Mathf.Clamp(pos.y, 50.0f, Screen.height - offset.y);



        rect.pivot = new Vector2(
            Mathf.InverseLerp(Screen.width * offset.x, Screen.width * (1 - offset.x), pos.x), 0);

        rect.position = pos;

        foreach (var dot in dots)
            dot.Update(pos, lum);

    }
}

[Serializable]
public struct BubbleDot
{
    public RectTransform go;
    [Range(0, 1)] public float lerpVal;

    public void Update(Vector3 anchorPos, Lumberjack lum)
    {
        Vector3 lumPos = Camera.main.WorldToScreenPoint(lum.transform.position + Vector3.up * 2.0f);
        lumPos.x = Mathf.Clamp(lumPos.x, 0, Screen.width);
        lumPos.y = Mathf.Clamp(lumPos.y, 0, Screen.height);
        Vector3 pos = Vector3.Lerp(anchorPos, lumPos, lerpVal);
        go.position = pos;
    }
}
