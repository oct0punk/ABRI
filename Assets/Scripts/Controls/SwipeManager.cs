using Unity.VisualScripting;
using UnityEngine;

public class SwipeManager : MonoBehaviour
{
    [SerializeField] private Swipe[] swipes;
    static SwipeManager instance;
    bool canConstruct = true;
    private static SwipeManager Instance
    {
        get
        {
            if (instance != null) {
                return instance;
            }
            else {
                throw new System.NotImplementedException("SwipeManager Instance");
            }
        }
        set { }
    }


    private void Start()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        foreach (var swipe in swipes) swipe.Update();

        if (!canConstruct)
            canConstruct = Input.touches.Length == 0;
    }

    #region Walk
    public static bool MoveLeft()
    {
        if (MoveCancel()) return false;
        return testLeft();
    }

    public static bool MoveRight()
    {
        if (MoveCancel()) return false;
        return testRight();
    }

    private static bool testLeft()
    {
        foreach (var t in Input.touches)
        {
            if (t.position.x < Screen.width * .2f) return true;
        }
        return Input.GetMouseButton(0) && Input.mousePosition.x < Screen.width * .2f;
    }
    private static bool testRight()
    {
        foreach (var t in Input.touches)
        {
            if (t.position.x > Screen.width * (1 - 0.2f)) return true;
        }
        return Input.GetMouseButton(0) && Input.mousePosition.x > Screen.width * (1 - 0.2f);
    }
    private static bool MoveCancel()
    {
        return testLeft() && testRight();
    }
    #endregion

    #region Climb
    public static bool ClimbUp()
    {
        int count = 0;
        foreach (var s in Instance.swipes)
        {
            if (s.SwipeUp(.5f)) ++count;
        }
        return count == 1;
    }
    public static bool ClimbDown()
    {        
        int count = 0;
        foreach (var s in Instance.swipes)
        {
            if (s.SwipeDown(.5f)) ++count;
        }
        return count == 1;
    }
    #endregion

    public static bool Cut()
    {
        foreach (var s in Instance.swipes)
        {
            if (s.SwipeLeft(0.0f)) return true;
            if (s.SwipeRight(0.0f)) return true;
            if (s.SwipeDown(0.0f)) return true;
            if (s.SwipeUp(0.0f)) return true;
        }
        return false;
    }
}
