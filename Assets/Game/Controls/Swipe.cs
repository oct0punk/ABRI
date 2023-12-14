using UnityEngine;

[System.Serializable]
public class Swipe
{
    private float tap, swipeUp, swipeDown, swipeLeft, swipeRight = 10.0f;
    private bool isDragging;
    public bool DoOnce { get { return iter == 1; } }
    private int iter = 0;
    private Vector2 startTouch, swipeDelta;

    public bool Tap(float tolerance) { return !isDragging && tap <= tolerance; }
    public bool SwipeUp(float tolerance) { return isDragging && swipeUp <= tolerance; }
    public bool SwipeDown(float tolerance) { return isDragging && swipeDown <= tolerance; }
    public bool SwipeLeft(float tolerance) { return isDragging && swipeLeft <= tolerance; }
    public bool SwipeRight(float tolerance) { return isDragging && swipeRight <= tolerance; }

    public int index;

    float distanceToDrag = 100.0f;



    // Update is called once per frame
    public void Update()
    {
        tap += Time.deltaTime;
        swipeDown += Time.deltaTime;
        swipeLeft += Time.deltaTime;
        swipeRight += Time.deltaTime;
        swipeUp += Time.deltaTime;

        if (Input.GetMouseButtonDown(index))
        {
            Debug.Log("click");
            startTouch = Input.mousePosition;
            isDragging = true;
        }
        if (Input.GetMouseButtonUp(index))
        {
            Debug.Log("end");
            Reset();
        }
        if (Input.GetMouseButton(index))
        {
            Debug.Log("hold");
            if (isDragging)
            {
                swipeDelta = (Vector2)Input.mousePosition - startTouch;

                if (swipeDelta.magnitude > distanceToDrag)
                {
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

                    iter++;
                    startTouch = Input.mousePosition;
                }
            }
        }

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

                if (swipeDelta.magnitude > distanceToDrag)
                {
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

                    iter++;
                    startTouch = Input.touches[index].position;
                }
            }
        }

    }

    private void Reset()
    {
        if (isDragging)
            tap = 0;
        isDragging = false;
        iter = 0;

        startTouch = swipeDelta = Vector2.zero;

    }
}
