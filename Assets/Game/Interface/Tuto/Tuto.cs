using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tuto : MonoBehaviour
{
    public Lumberjack lum;

    [Header("Cut Tuto")]
    public static bool slideDone = false;
    public GameObject slideTuto;
    
    [Header("Climb Tuto")]
    public static bool canClimb = false;
    public static bool climbDownDone;
    public GameObject climbDownTuto;

    [Header("BuildTuto")]
    public static bool canBuild = false;
    public static bool buildDone;
    public GameObject buildTuto;
    public GameObject dragNdrop;
    public GameObject moveTuto;

    private void Start()
    {
        // StartCoroutine(WalkTuto());
        StartCoroutine(CutTuto());
        StartCoroutine(ClimbTuto());
        StartCoroutine(BuildTuto());
    }

    IEnumerator WalkTuto()
    {
        yield return new WaitUntil(() => GameManager.instance.lumberjack.fsm == GameManager.instance.lumberjack.movingState);
        moveTuto.SetActive(false);
    }

    IEnumerator CutTuto()
    {
        while (!slideDone)
        {
            slideTuto.SetActive(false);
            yield return new WaitUntil(() => lum.canCutRes != null);
            yield return new WaitUntil(() => lum.canCutRes.Count > 0);
            slideTuto.SetActive(true);
            yield return new WaitUntil(() => lum.pickingResource != null || lum.canCutRes.Count == 0);
            if (lum.pickingResource != null)
            {
                yield return new WaitWhile(() => lum.pickingResource.alive);
                slideDone = true;
                StartCoroutine(LootTuto());
            }
            else
            {
            }
            slideTuto.SetActive(false);
        }
    }
    IEnumerator LootTuto()
    {
        yield return null;
    }

    IEnumerator ClimbTuto()
    {
        while (!climbDownDone)
        {
            climbDownTuto.SetActive(false);
            yield return new WaitWhile(() => !canClimb);
            climbDownTuto.SetActive(true);

            yield return new WaitUntil(() => !canClimb || lum.fsm == lum.climbingState);
            if (lum.fsm == lum.climbingState)
            {
                climbDownDone = true;
            }
            else
            {
            }
            climbDownTuto.SetActive(false);
        }
    }

    IEnumerator BuildTuto()
    {
        while (!buildDone)
        {
            buildTuto.SetActive(false);
            dragNdrop.SetActive(false);
            yield return new WaitWhile(() => !canBuild);
            buildTuto.SetActive(true);
            yield return new WaitUntil(() => lum.workingState.state == WorkState.Building || !canBuild);
            if (lum.workingState.state == WorkState.Building)
            {
                buildTuto.SetActive(false);
                dragNdrop.SetActive(true);
                yield return new WaitWhile(() => !buildDone);
                buildTuto.SetActive(false);
                dragNdrop.SetActive(false);
            }
            else
            {

            }

        }
    }
}
