using System;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering.Universal;
using Unity.VisualScripting;
using UnityEngine.UI;

[SelectionBase]
public class Shelter : MonoBehaviour
{
    static Shelter instance;

    [Header("Enter")]
    [SerializeField] SpriteRenderer ext;
    [SerializeField] Light2D light;
    [SerializeField] CinemachineVirtualCamera cam;

    [Header("Temperature")]
    [SerializeField] float temperature = 20.0f;
    [SerializeField] float timeBeforeNextGust = 1.0f;
    [SerializeField] int push = 0;

    Piece[] pieces;
    Storm storm;
    public Slider thermometer;


    void Awake()
    {
        instance = this;
        storm = GetComponent<Storm>();
        pieces = GetComponentsInChildren<Piece>();        
    }



    private void Update()
    {
        ChangeTemperature(push * Time.deltaTime);

        timeBeforeNextGust -= Time.deltaTime;
        if (timeBeforeNextGust < 0.0f)
        {
            timeBeforeNextGust = 5.0f; // UnityEngine.Random.Range(40.0f, 100.0f);
            foreach (Piece p in pieces) {
                p.Resist(storm.wind);
        }   }

    }


    void ChangeTemperature(float amount)
    {        
        temperature = Mathf.Clamp(temperature + amount, 0.0f, 25.0f);        
        if (temperature <= 0.0f)
        {
            GameManager.instance.GameOver();
            return;
        }
        
        
        thermometer.value = Mathf.Lerp(0.0f, 1.0f, temperature / 20.0f);
    }

    public static void UpdateSpeed(int amount)
    {
        instance.push += amount;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Le bucheron ENTRE dans la cabane
        if (collision.GetComponentInParent<Lumberjack>() != null)
        {
            ext.enabled = false;
            light.enabled = true;
            CameraManager.Possess(cam);
            Array.ForEach<Piece>(instance.pieces, p => p.SetBubbleVisible(true));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Le bucheron SORT
        Lumberjack lum = collision.GetComponentInParent<Lumberjack>();
        if (lum != null)
        {
            ext.enabled = true;
            CameraManager.Possess(lum.cam);
            light.enabled = false;
            Array.ForEach<Piece>(instance.pieces, p => p.SetBubbleVisible(false));
        }
    }
}
