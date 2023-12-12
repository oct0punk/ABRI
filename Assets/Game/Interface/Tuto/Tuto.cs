using Cinemachine;
using System;
using System.Collections;
using UnityEngine;

public class Tuto : MonoBehaviour
{
    public static bool tutoCut = true;
    public static bool tutoClimb = true;

    public static bool canBuild = true;
    public static bool canCraft = true;
    public static bool updateWind = true;

    public static ICinemachineCamera pieceCam;
    public repairTutoTrail trail;
    public TapBubble closePlans;


    public void Launch(Lumberjack lum)
    {
        StartCoroutine(FirstTuto(lum));
    }

    public IEnumerator FirstTuto(Lumberjack lum)
    {
        updateWind = false;
        canBuild = false;
        canCraft = false;

        GameUI ui = GameManager.instance.ui;
        // Apprendre à alimenter la cheminée

        // Utiliser l'établi pour réparer l'abri

        // Construire ponts et échelles

        yield return lum.Message(ui.GetBubbleContentByName("shelterCold"), 2.0f);
       
        // TutoMove
        ui.BothMove();
        GameManager.instance.lumberjack.enabled = true;        
        yield return lum.Message(ui.GetBubbleContentByName("leftArrow"), () => lum.fsm == lum.idleState);        
        yield return new WaitForSeconds(1);

        bool cutTuto = false;
        while (!cutTuto)
        {
            yield return lum.Message(ui.GetBubbleContentByName("cutLog"), 1.0f);
            yield return new WaitUntil(() => lum.storage.Count(RawMatManager.instance.GetRawMatByName("Log")) > 0 || lum.indoor);
            if (!lum.indoor)
            {
                cutTuto = true;
            }

        }

        // Tuto Chimney
        Chimney chimney = GameManager.instance.shelter.chimney;
        chimney.bubble.touchTuto.SetActive(true);
        bool chimneyTuto = false;
        while (!chimneyTuto)
        {
            yield return lum.Message(ui.GetBubbleContentByName("rightArrowToShelter"), () => !lum.indoor);
            yield return new WaitForSeconds(.3f);
            yield return new WaitWhile(() => chimney.GetPower() <= 0 && lum.indoor);
            if (lum.indoor)
            {
                yield return new WaitForSeconds(.3f);
                yield return lum.Message(ui.GetBubbleContentByName("HotChimney"), 1.0f);
                chimneyTuto = true;
            }
        }
        chimney.bubble.touchTuto.SetActive(false);

        yield return new WaitForSeconds((float)chimney.tl.duration);

        Debug.Log("EndTutoChimney");


        // Le bûcheron réfléchit, puis trouve une idée
        yield return lum.Message(ui.GetBubbleContentByName("think"), 1.5f);
        yield return new WaitForSeconds(.1f);
        yield return lum.Message(ui.GetBubbleContentByName("light"), 1.0f);
        yield return lum.Message(ui.GetBubbleContentByName("cutBranch"), () => lum.storage.Count(RawMatManager.instance.GetRawMatByName("Branch")) <= 0);
        
        // TutoCraft
        canCraft = true;
        Workbench workbench = GameManager.instance.shelter.workbench;
        bool craftTuto = false;
        while (!craftTuto)
        {
            yield return lum.Message(ui.GetBubbleContentByName("rightArrowToShelter"), () => !lum.indoor);
            yield return new WaitForSeconds(.3f);
            workbench.openBubble.touchTuto.SetActive(true);
            yield return lum.Message(ui.GetBubbleContentByName("workbench"), () => GameManager.instance.gameState != GameState.Craft && lum.indoor);
            if (lum.indoor)
            {   // Tuto workbench
                workbench.closeBubble.gameObject.SetActive(false);
                CraftBubble planchBubble = Array.Find(workbench.plans.GetComponentsInChildren<CraftBubble>(), b => b.material == RawMatManager.instance.GetRawMatByName("WoodPlanch"));
                planchBubble.touchTuto.SetActive(true);
                planchBubble.craftButton.touchTuto.SetActive(true);
                yield return new WaitUntil(() => lum.storage.Count(RawMatManager.instance.GetRawMatByName("WoodPlanch")) > 0);
                planchBubble.craftButton.touchTuto.SetActive(false);
                craftTuto = true;
            }
        }
        workbench.closeBubble.gameObject.SetActive(true);
        workbench.closeBubble.touchTuto.SetActive(true);
        workbench.openBubble.touchTuto.SetActive(false);
        yield return new WaitWhile(() => GameManager.instance.gameState == GameState.Craft);
        workbench.closeBubble.touchTuto.SetActive(false);
        Debug.Log("EndTutoCraft");

        // Tuto Piece
        canBuild = true;
        lum.openPlans.touchTuto.SetActive(true);
        yield return new WaitUntil(() => GameManager.instance.gameState == GameState.Build);
        trail.gameObject.SetActive(true);
        yield return new WaitUntil(() => trail.piece.GetComponent<Piece>().alive);
        Destroy(trail.gameObject);
        closePlans.touchTuto.SetActive(true);
        yield return new WaitWhile(() => GameManager.instance.gameState == GameState.Build);
        closePlans.touchTuto.SetActive(false);
        Debug.Log("EndTutoPiece");

        updateWind = true;
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
