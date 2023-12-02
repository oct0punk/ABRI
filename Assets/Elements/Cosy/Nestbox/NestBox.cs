using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class NestBox : MonoBehaviour
{
    [SerializeField] private Sprite sprite;
    public ConsumeBubble bubble;
    public Bird bird;
    public bool isBuilt;

    // Start is called before the first frame update
    void Start()
    {
        bubble.action = Build;
        bubble.gameObject.SetActive(false);
    }

    public void Build()
    {
        transform.localScale = Vector3.one;
        GetComponentInChildren<SpriteRenderer>().sprite = sprite;
        bird.perch = this;
        isBuilt = true;
        GameManager.instance.OnNestBuilt();
    }
}
