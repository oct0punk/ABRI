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
    public static bool skip = false;

    public static bool canBuild = true;
    public static bool canCraft = true;
    public static bool updateWind = true;

    public repairTutoTrail pieceTrail;
    public repairTutoTrail bridgeTrail;
    public TapBubble closePlans;

    [Header("CameraSpots")]
    public CinemachineVirtualCamera shelterCam;
    public CinemachineVirtualCamera woodCam;
    public CinemachineVirtualCamera chimneyCam;
    public CinemachineVirtualCamera pieceCam;

    [Header("PlayerPos")]
    public Transform chimneyPos;
    public Transform workbenchPos;



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
        // First Cine
        {
            yield return new WaitForSeconds(CameraManager.Possess(shelterCam));
            yield return lum.Message(ui.GetBubbleContentByName("shelterCold"), () => Input.touches.Length == 0);
            lum.AutoMoveTo(chimneyPos.position, () => { lum.spriteRenderer.flipX = true; });
            yield return new WaitWhile(() => lum.isAutoMoving);

            yield return new WaitForSeconds(CameraManager.Possess(chimneyCam));
            yield return lum.Message(ui.GetBubbleContentByName("chimneyOff"), () => Input.touches.Length == 0);
            yield return new WaitForSeconds(CameraManager.Possess(woodCam));
            yield return lum.Message(ui.GetBubbleContentByName("burn"), () => Input.touches.Length == 0);
            yield return new WaitForSeconds(CameraManager.Possess(lum.cam));
        }

        // TutoMove
        {
            ui.BothMove();
            lum.enabled = true;
            yield return lum.Message(ui.GetBubbleContentByName("leftArrow"), () => lum.fsm == lum.idleState);
            yield return new WaitForSeconds(1);
        }

        // TutoCut
        {
            bool cutTuto = false;
            while (!cutTuto)
            {
                yield return lum.Message(ui.GetBubbleContentByName("cutLog"), 1.0f);
                yield return new WaitUntil(() => lum.storage.Count(RawMatManager.instance.GetRawMatByName("Branch")) > 1
                                              && lum.storage.Count(RawMatManager.instance.GetRawMatByName("Log")) > 0 || lum.indoor);

                if (!lum.indoor)
                {
                    cutTuto = true;
                }
            }
        }

        // Tuto Chimney
        {
            Chimney chimney = GameManager.instance.shelter.chimney;
            chimney.bubble.touchTuto.SetActive(true);
            bool chimneyTuto = false;
            chimney.bubble.action = () => { chimneyTuto = true; };
            while (!chimneyTuto)
            {
                yield return lum.Message(ui.GetBubbleContentByName("rightArrowToShelter"), () => !lum.indoor);
                yield return new WaitForSeconds(.3f);
                yield return new WaitUntil(() => chimneyTuto || !lum.indoor);

                // ChimneyCine
                if (chimneyTuto)
                {
                    chimney.bubble.touchTuto.SetActive(false);
                    chimney.bubble.gameObject.SetActive(false);
                    lum.AutoMoveTo(chimneyPos.position, () => { lum.spriteRenderer.flipX = true; lum.enabled = false; });
                    yield return new WaitForSeconds(CameraManager.Possess(chimney.cam));
                    chimney.Reload();
                    yield return new WaitForSeconds(.3f);
                    yield return lum.Message(ui.GetBubbleContentByName("HotChimney"), () => Input.touches.Length == 0);

                    chimney.bubble.action = () => { chimney.Reload(); };
                    chimneyTuto = true;
                }
            }
        }

        #endregion

        #region  Réparer le trou

        // TutoCraft
        {
            yield return new WaitForSeconds(CameraManager.Possess(pieceCam));
            yield return new WaitForSeconds(1.0f);
            Piece p = GameManager.instance.shelter.pieces[0];
            yield return new WaitForSeconds(1.0f);
            p.Break();

            yield return new WaitForSeconds(CameraManager.Possess(lum.cam));
            yield return lum.Message(ui.GetBubbleContentByName("holeRefreshShelter"), () => Input.touches.Length == 0);
            yield return lum.Message(ui.GetBubbleContentByName("TransformWood"), () => Input.touches.Length == 0);

            lum.AutoMoveTo(workbenchPos.position);
            yield return new WaitWhile(() => lum.isAutoMoving);

            Workbench workbench = GameManager.instance.shelter.workbench;
            canCraft = true;
            workbench.openBubble.gameObject.SetActive(true);

            // Craft Planch
            workbench.openBubble.touchTuto.SetActive(true);
            workbench.closeBubble.gameObject.SetActive(false);
            CraftBubble planchBubble = Array.Find(workbench.plans.GetComponentsInChildren<CraftBubble>(), b => b.material == RawMatManager.instance.GetRawMatByName("WoodPlanch"));
            planchBubble.touchTuto.SetActive(true);
            planchBubble.craftButton.touchTuto.SetActive(true);
            yield return new WaitUntil(() => lum.storage.Count(RawMatManager.instance.GetRawMatByName("WoodPlanch")) > 0);
            planchBubble.craftButton.touchTuto.SetActive(false);

            // Exit workbench
            workbench.closeBubble.gameObject.SetActive(true);
            workbench.closeBubble.touchTuto.SetActive(true);
            workbench.openBubble.touchTuto.SetActive(false);
            yield return new WaitWhile(() => GameManager.instance.gameState == GameState.Craft);
            workbench.closeBubble.touchTuto.SetActive(false);
        }


        // TutoRepair
        {
            canBuild = true;
            lum.openPlans.gameObject.SetActive(true);
            lum.openPlans.touchTuto.SetActive(true);
            yield return lum.Message(ui.GetBubbleContentByName("RepairPiece"), () => GameManager.instance.gameState != GameState.Build);

            pieceTrail.gameObject.SetActive(true);
            yield return new WaitUntil(() => pieceTrail.target.GetComponent<Piece>().alive);

            Destroy(pieceTrail.gameObject);
            closePlans.touchTuto.SetActive(true);
            yield return new WaitWhile(() => GameManager.instance.gameState == GameState.Build);
            closePlans.touchTuto.SetActive(false);

            lum.openPlans.touchTuto.SetActive(false);
        }
        #endregion

        lum.enabled = true;

        RawMatManager.instance.AddBubbleToWorkbench("Bridge");
        RawMatManager.instance.AddBubbleToWorkbench("Ladder");

        updateWind = true;
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
