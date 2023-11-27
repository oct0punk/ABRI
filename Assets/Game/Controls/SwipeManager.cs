using Unity.VisualScripting;
using UnityEngine;

public class SwipeManager : MonoBehaviour
{
    [SerializeField] private Swipe[] swipes;
    public bool buildLadder = false;
    public bool construct = false;
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

    public static bool GetBuildLadder()
    {
        if (Instance.buildLadder)
        {
            Instance.buildLadder = false;
            return true;
        }
        return false;
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
            if (t.position.x < Screen.width / 4) return true;
        }
        return false;
    }
    private static bool testRight()
    {
        foreach (var t in Input.touches)
        {
            if (t.position.x > Screen.width * 0.75f) return true;
        }
        return false;
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

    #region Actions
    public static bool Cut()
    {
        foreach (var s in Instance.swipes)
        {
            if (s.SwipeLeft(0.0f)) return true;
            if (s.SwipeRight(0.0f)) return true;
        }
        return false;
    }

    public static bool ConstructMode()
    {
        if (instance.construct) {
            instance.construct = false;
            return true;
        }
        if (!instance.canConstruct) return false;
        if (instance.swipes.Length < 2) return false;

        int countDown = 0;
        bool foundZero = false;
        foreach (var s in Instance.swipes)
        {
            if (s.SwipeDown(.4f)) {
                countDown++;
                if (s.SwipeDown(0.0f)) foundZero = true;
            }
        }
        
        bool res = countDown > 1 && foundZero;
        if (res)
            instance.canConstruct = false;
        return res;
    }
    #endregion
}
