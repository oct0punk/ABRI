using UnityEngine;

public class Menu : MonoBehaviour
{
    public void NewGame()
    {
        GameManager.instance.Launch();
    }

    public void Options()
    {

    }

    public void Credits()
    {

    }

    public void Exit()
    {
        Application.Quit();
    }
}
