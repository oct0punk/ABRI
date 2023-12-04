using System;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using Unity.VisualScripting;

[SelectionBase]
public class Shelter : MonoBehaviour
{
    [Header("Enter")]
    [SerializeField] SpriteRenderer ext;
    [SerializeField] new Light2D light;
    public CinemachineVirtualCamera cam;
    [Space]
    [Header("Temperature")]
    [SerializeField] float temperature = 20.0f;
    [SerializeField] float maxTemperature = 25.0f;
    [SerializeField] float speed = .5f;
    [SerializeField] float timeBeforeNextGust = 1.0f;
    [SerializeField] int push = 0;
    [SerializeField] Slider thermometer;
    Storm storm;
    public Storage storage { get; private set; }
    public Piece[] pieces;
    public Workbench workbench;
    public Chimney chimney;

    [Space]
    public NestBox[] perchs;


    void Awake()
    {
        storm = GetComponent<Storm>();
        pieces = GetComponentsInChildren<Piece>();
        storage = GetComponentInChildren<Storage>();
        perchs = GetComponentsInChildren<NestBox>();
    }



    private void Update()
    {
        ChangeTemperature(push * Time.deltaTime * speed);

        timeBeforeNextGust -= Time.deltaTime;
        if (timeBeforeNextGust < 0.0f)
        {
            timeBeforeNextGust = UnityEngine.Random.Range(40.0f, 100.0f);
            foreach (Piece p in pieces)
            {
                p.Resist(storm.wind);
            }
        }

    }


    void ChangeTemperature(float amount)
    {
        temperature = Mathf.Clamp(temperature + amount, 0.0f, maxTemperature);
        if (temperature <= 0.0f)
        {
            GameManager.instance.GameOver();
            return;
        }


        thermometer.value = Mathf.Lerp(0.0f, 1.0f, temperature / maxTemperature);
    }
    public static void UpdateSpeed(int amount)
    {
        GameManager.instance.shelter.push += amount;
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Le bucheron ENTRE dans la cabane
        Lumberjack lum = collision.GetComponentInParent<Lumberjack>();
        if (lum != null)
        {
            GameManager.instance.ChangeState(GameState.Indoor);
            OnEnter();

            // Free the birds
            if (lum.carryingBird != null)
            {
                Bird bird = lum.carryingBird;
                bird.perch = perchs[0].GetComponent<NestBox>();
                bird.Free();
                lum.carryingBird = null;
            }

            // Store planchs
            RawMaterial mat = RawMatManager.instance.GetRawMatByName("WoodPlanch");
            int iter = lum.storage.Count(mat);
            if (iter == 0) return;
            Debug.Log("Lum enter : " + iter + " empty emplacements");
            lum.storage.Add(mat, -iter);
            storage.Add(mat, iter);
        }
    }
    void OnEnter()
    {
        // Visibility
        ext.enabled = false;
        light.enabled = true;
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        // Le bucheron SORT
        Lumberjack lum = collision.GetComponentInParent<Lumberjack>();
        if (lum != null)
        {
            GameManager.instance.ChangeState(GameState.Explore);
            OnExit();

            // Restore planchs
            RawMaterial material = RawMatManager.instance.GetRawMatByName("WoodPlanch");
            int iter = Mathf.Min(storage.Count(material), lum.storage.CountEmpty(material));
            if (iter == 0) return;
            lum.storage.Add(material, iter);
            storage.Add(material, -iter);
        }
    }
    void OnExit()
    {
        ext.enabled = true;
        light.enabled = false;
    }


    public void DisplayPieceBubble(bool visible)
    {
        SetNestsBubbleVisibility(false);
        SetPiecesBubbleVisibility(visible);
    }
    void SetPiecesBubbleVisibility(bool visible)
    {
        foreach (Piece p in pieces)
        {
            p.SetBubbleVisible(visible);
        }
    }

    public void DisplayNestsBubble(bool visible)
    {
        SetPiecesBubbleVisibility(false);
        SetNestsBubbleVisibility(visible);
    }
    void SetNestsBubbleVisibility(bool visible)
    {
        foreach (NestBox n in perchs)
        {
            n.bubble.gameObject.SetActive(visible);
        }
    }
}