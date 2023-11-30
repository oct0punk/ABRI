using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NestBox : MonoBehaviour
{
    [SerializeField] private Sprite sprite;
    [SerializeField] private ConsumeBubble bubble;
    [SerializeField] Bird bird;

    // Start is called before the first frame update
    void Start()
    {
        bubble.action = Build;
    }

    public void Build()
    {
        transform.localScale = Vector3.one;
        GetComponent<SpriteRenderer>().sprite = sprite;
        bird.perch = this;
    }
}
