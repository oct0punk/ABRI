using Cinemachine;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Tuto : MonoBehaviour
{
    public static bool tutoCut = true;
    public static bool tutoClimb = true;
    public static bool tutoBuildBridge = true;

    public static bool canBuild = true;
    public static bool canCraft = true;
    public static bool updateWind = true;

    public static ICinemachineCamera pieceCam;
    public repairTutoTrail pieceTrail;
    public repairTutoTrail bridgeTrail;
    public TapBubble closePlans;


    public void Launch(Lumberjack lum)
    {
        StartCoroutine(FirstTuto(lum));
    }

    public IEnumerator FirstTuto(Lumberjack lum)
    {
        tutoBuildBridge = false;
        updateWind = false;
        canBuild = false;
        canCraft = false;

        GameUI ui = GameManager.instance.ui;
        #region  Apprendre à alimenter la cheminée


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
                yield return new WaitForSeconds(1.3f);
                lum.Message(ui.GetBubbleContentByName("HotChimney"), 1.0f);
                chimneyTuto = true;
            }
        }
        chimney.bubble.touchTuto.SetActive(false);

        yield return new WaitForSeconds((float)chimney.tl.duration);

        #endregion


        #region  Utiliser l'établi
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
        #endregion

        #region  pour réparer l'abri

        canBuild = true;
        lum.openPlans.touchTuto.SetActive(true);
        yield return new WaitUntil(() => GameManager.instance.gameState == GameState.Build);
        pieceTrail.gameObject.SetActive(true);
        yield return new WaitUntil(() => pieceTrail.target.GetComponent<Piece>().alive);
        Destroy(pieceTrail.gameObject);
        closePlans.touchTuto.SetActive(true);
        yield return new WaitWhile(() => GameManager.instance.gameState == GameState.Build);
        closePlans.touchTuto.SetActive(false);
        
        lum.openPlans.touchTuto.SetActive(false);
        #endregion

        updateWind = true;

        RawMatManager.instance.AddBubbleToWorkbench("Bridge");
        RawMatManager.instance.AddBubbleToWorkbench("Ladder");

        tutoBuildBridge = true;
    }

    public IEnumerator BuildBridgeTuto(Lumberjack lum, AnchorForBridge bridge)
    {
        tutoBuildBridge = false;
        canBuild = false;
        GameUI ui = GameManager.instance.ui;
        // Posséder un bridge
        Func<bool> cond = () => lum.storage.Count(RawMatManager.instance.GetRawMatByName("Bridge")) <= 0;
        yield return lum.Message(ui.GetBubbleContentByName("BuildBridge"), 2.0f);
        while (cond.Invoke())
        {
            yield return new WaitForSeconds(.2f);
            lum.Message(ui.GetBubbleContentByName("rightArrowToShelter"), () => !lum.indoor);
            yield return new WaitWhile(() => !lum.indoor);
            yield return new WaitForSeconds(.2f);
            lum.Message(ui.GetBubbleContentByName("CraftBridge"), () => lum.indoor && GameManager.instance.gameState != GameState.Craft);
            yield return new WaitWhile(() => lum.indoor && cond.Invoke());
        }
        // Construire le pont
        canBuild = true;
        lum.Message(ui.GetBubbleContentByName("leftArrow"), () => lum.indoor);
        yield return new WaitWhile(() => lum.indoor);
        lum.openPlans.touchTuto.SetActive(true);
        yield return new WaitUntil(() => GameManager.instance.gameState == GameState.Build);
        bridgeTrail.gameObject.SetActive(true);
        yield return new WaitUntil(() => bridge.isBuilt);
        Destroy(bridgeTrail.gameObject);
        lum.openPlans.touchTuto.SetActive(false);

    }
}
