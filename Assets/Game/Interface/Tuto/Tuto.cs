using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tuto : MonoBehaviour
{
    public static bool canBuild = true;


    public void Launch(Lumberjack lum)
    {
        StartCoroutine(FirstTuto(lum));
    }

    public IEnumerator FirstTuto(Lumberjack lum)
    {
        canBuild = false;
        yield return lum.Message(GameUI.instance.GetBubbleContentByName("shelterCold"), 2.0f);
        Workbench workbench = GameManager.instance.shelter.workbench;
        workbench.GetComponent<Collider2D>().enabled = false;
       
        // TutoMove
        GameUI.instance.BothMove();
        GameManager.instance.lumberjack.enabled = true;        
        yield return lum.Message(GameUI.instance.GetBubbleContentByName("leftArrow"), () => lum.fsm == lum.idleState);        
        yield return new WaitForSeconds(1);        
    
        // TutoRes
        yield return lum.Message(GameUI.instance.GetBubbleContentByName("seekBranch"), 1.0f);
        yield return new WaitUntil(() => lum.storage.Count(RawMatManager.instance.GetRawMatByName("WoodBranch")) > 0);        
        yield return lum.Message(GameUI.instance.GetBubbleContentByName("rightArrowToShelter"), () => !lum.indoor);
        yield return new WaitForSeconds(.3f);

        // TutoCraft
        workbench.GetComponent<Collider2D>().enabled = true;
        workbench.openBubble.touchTuto.SetActive(true);
        yield return lum.Message(GameUI.instance.GetBubbleContentByName("workbench"), () => GameManager.instance.gameState != GameState.Craft);
  
        workbench.swapBubble.gameObject.SetActive(false);
        workbench.closeBubble.gameObject.SetActive(false);
        workbench.openBubble.touchTuto.SetActive(false);
        yield return new WaitUntil(() => lum.storage.Count(RawMatManager.instance.GetRawMatByName("WoodPlanch")) > 0);
     
        // TutoSwapMode
        workbench.swapBubble.gameObject.SetActive(true);
        workbench.swapBubble.touchTuto.SetActive(true);
        yield return new WaitUntil(() => workbench.state == CraftState.Piece);
        workbench.swapBubble.touchTuto.SetActive(false);

        // TutoPiece
        Piece p = Array.Find(GameManager.instance.shelter.pieces, p => !p.alive);
        p.bubble.touchTuto.SetActive(true);
        yield return new WaitUntil(() => p.alive);
        p.bubble.touchTuto.SetActive(false);
     
        // Exit
        workbench.closeBubble.gameObject.SetActive(true);
        workbench.closeBubble.touchTuto.SetActive(true);
        yield return new WaitUntil(() => GameManager.instance.gameState != GameState.Craft);
        workbench.closeBubble.touchTuto.SetActive(false);


        // Tuto chimney ?
    }

    //public Lumberjack lum;

    //[Header("Cut Tuto")]
    //public static bool slideDone = false;
    //public GameObject slideTuto;
    
    //[Header("Climb Tuto")]
    //public static bool canClimb = false;
    //public static bool climbDownDone;
    //public GameObject climbDownTuto;

    //[Header("BuildTuto")]
    //public static bool canBuild = false;
    //public static bool buildDone;
    //public GameObject buildTuto;
    //public GameObject dragNdrop;
    //public GameObject moveTuto;

    //private void Start()
    //{
    //    // StartCoroutine(WalkTuto());
    //    StartCoroutine(CutTuto());
    //    StartCoroutine(ClimbTuto());
    //    StartCoroutine(BuildTuto());
    //}

    //IEnumerator WalkTuto()
    //{
    //    yield return new WaitUntil(() => GameManager.instance.lumberjack.fsm == GameManager.instance.lumberjack.movingState);
    //    moveTuto.SetActive(false);
    //}

    //IEnumerator CutTuto()
    //{
    //    while (!slideDone)
    //    {
    //        slideTuto.SetActive(false);
    //        yield return new WaitUntil(() => lum.canCutRes != null);
    //        yield return new WaitUntil(() => lum.canCutRes.Count > 0);
    //        slideTuto.SetActive(true);
    //        yield return new WaitUntil(() => lum.pickingResource != null || lum.canCutRes.Count == 0);
    //        if (lum.pickingResource != null)
    //        {
    //            yield return new WaitWhile(() => lum.pickingResource.alive);
    //            slideDone = true;
    //            StartCoroutine(LootTuto());
    //        }
    //        else
    //        {
    //        }
    //        slideTuto.SetActive(false);
    //    }
    //}
    //IEnumerator LootTuto()
    //{
    //    yield return null;
    //}

    //IEnumerator ClimbTuto()
    //{
    //    while (!climbDownDone)
    //    {
    //        climbDownTuto.SetActive(false);
    //        yield return new WaitWhile(() => !canClimb);
    //        climbDownTuto.SetActive(true);

    //        yield return new WaitUntil(() => !canClimb || lum.fsm == lum.climbingState);
    //        if (lum.fsm == lum.climbingState)
    //        {
    //            climbDownDone = true;
    //        }
    //        else
    //        }
    //        climbDownTuto.SetActive(false);
    //    }
    //}

    //IEnumerator BuildTuto()
    //{
    //    while (!buildDone)
    //    {
    //        buildTuto.SetActive(false);
    //        dragNdrop.SetActive(false);
    //        yield return new WaitWhile(() => !canBuild);
    //        buildTuto.SetActive(true);
    //        yield return new WaitUntil(() => lum.workingState.state == WorkState.Building || !canBuild);
    //        if (lum.workingState.state == WorkState.Building)
    //        {
    //            buildTuto.SetActive(false);
    //            dragNdrop.SetActive(true);
    //            yield return new WaitWhile(() => !buildDone);
    //            buildTuto.SetActive(false);
    //            dragNdrop.SetActive(false);
    //        }
    //        else
    //        {

    //        }

    //    }
    //}
}
