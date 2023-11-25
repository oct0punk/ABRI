using UnityEngine;

public class SwipeManager : MonoBehaviour
{
    public bool buildLadder = false;
    static SwipeManager instance;
    bool flag = true;
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

    [SerializeField] private Swipe[] swipes;

    private void Start()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        foreach (var swipe in swipes) swipe.Update();

        if (!flag)
        {
            flag = Input.touches.Length == 0;
        }
    }

    #region Movement
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
        if (instance.swipes.Length < 2) return false;
        if (!instance.flag) return false;

        int countDown = 0;
        foreach (var s in Instance.swipes)
        {
            if (s.SwipeDown(1.0f)) {
                countDown++;                
            }
        }
        bool res = countDown > 1;
        if (res)
            instance.flag = false;
        return res;
    }
}
