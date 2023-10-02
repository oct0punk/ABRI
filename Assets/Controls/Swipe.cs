using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Swipe
{
    private float tap, swipeUp, swipeDown, swipeLeft, swipeRight;
    private bool isDragging;
    private Vector2 startTouch, swipeDelta;

    public bool Tap(float tolerance) { return tap <= tolerance; }
    public bool SwipeUp(float tolerance) { return swipeUp <= tolerance; }
    public bool SwipeDown(float tolerance) { return swipeDown <= tolerance; }
    public bool SwipeLeft(float tolerance) { return swipeLeft <= tolerance; }
    public bool SwipeRight(float tolerance) { return swipeRight <= tolerance; }

    public int index;



    // Update is called once per frame
    public void Update()
    {
        tap += Time.deltaTime;
        swipeDown += Time.deltaTime;
        swipeLeft += Time.deltaTime;
        swipeRight += Time.deltaTime;
        swipeUp += Time.deltaTime;

        if (Input.touches.Length > index)
        {
            switch (Input.touches[index].phase)
            {
                case TouchPhase.Began:
                    startTouch = Input.touches[index].position;
                    isDragging = true;
                    break;
                case TouchPhase.Ended:
                    Reset();
                    break;
                case TouchPhase.Canceled:
                    Reset();
                    break;
            }

            if (isDragging)
            {
                swipeDelta = Input.touches[index].position - startTouch;

                if (swipeDelta.magnitude > 100)
                {
                    isDragging = false;

                    // which directions ?
                    float x = swipeDelta.x;
                    float y = swipeDelta.y;

                    if (Mathf.Abs(x) > Mathf.Abs(y))
                    {
                        // Left or Right
                        if (x < 0)
                            swipeLeft = 0;
                        else
                            swipeRight = 0;
                    }
                    else
                    {
                        // Up or Down
                        if (y < 0)
                            swipeDown = 0;
                        else
                            swipeUp = 0;
                    }

                    Reset();
                }
            }
        }
    }

    private void Reset()
    {
        if (isDragging)
            tap = 0;
        isDragging = false;

        startTouch = swipeDelta = Vector2.zero;

    }
}
