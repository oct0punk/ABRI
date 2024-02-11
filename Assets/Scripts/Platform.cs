using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private void Awake()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        box.size = new Vector2(renderer.size.x, 1);
    }
}
