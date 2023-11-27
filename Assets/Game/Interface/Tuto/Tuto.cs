using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tuto : MonoBehaviour
{
    public Lumberjack lum;

    [Header("Cut Tuto")]
    public bool slideDone = false;
    public GameObject slideTuto;
    
    [Header("Climb Tuto")]
    public bool climbDownDone;
    public GameObject climbDownTuto;
    public static bool canClimb = false;

    private void Start()
    {
        StartCoroutine(CutTuto());
        StartCoroutine(ClimbTuto());
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
}
