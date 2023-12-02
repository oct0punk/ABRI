using System;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

[SelectionBase]
public class Shelter : MonoBehaviour
{
    static Shelter instance;

    [Header("Enter")]
    [SerializeField] SpriteRenderer ext;
    [SerializeField] new Light2D light;
    [SerializeField] CinemachineVirtualCamera cam;
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
    
    [Space]
    public NestBox[] perchs;    


    void Awake()
    {
        instance = this;
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
        instance.push += amount;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Le bucheron ENTRE dans la cabane
        Lumberjack lum = collision.GetComponentInParent<Lumberjack>();
        if (lum != null)
        {
            lum.indoor = true;

            // Free the birds
            if (lum.carryingBird != null)
            {
                Bird bird = lum.carryingBird;
                bird.perch = perchs[0].GetComponent<NestBox>();
                bird.Free();
                lum.carryingBird = null;
            }

            // Visibility
            ext.enabled = false;
            light.enabled = true;
            CameraManager.Possess(cam);

            // Store planchs
            RawMaterial mat = RawMatManager.instance.GetRawMatByName("WoodPlanch");
            int iter = lum.storage.Count(mat);
            if (iter == 0) return;
            Debug.Log("Lum enter : " + iter + " empty emplacements");
            lum.storage.Add(mat, -iter);
            storage.Add(mat, iter);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Le bucheron SORT
        Lumberjack lum = collision.GetComponentInParent<Lumberjack>();
        if (lum != null)
        {
            lum.indoor = false;

            ext.enabled = true;
            CameraManager.Possess(lum.cam);
            light.enabled = false;

            // Restore planchs
            RawMaterial material = RawMatManager.instance.GetRawMatByName("WoodPlanch");
            int iter = Mathf.Min(storage.Count(material), lum.storage.CountEmpty(material));
            if (iter == 0) return;
            Debug.Log("Lum exit : " + iter + " branchs to move");
            lum.storage.Add(material, iter);
            storage.Add(material, -iter);
        }
    }
}
