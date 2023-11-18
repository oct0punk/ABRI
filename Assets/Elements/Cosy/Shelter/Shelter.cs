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

    [SerializeField] SpriteRenderer ext;
    [SerializeField] Light2D light;
    [SerializeField] CinemachineVirtualCamera cam;

    [SerializeField] float temperature = 20.0f;
    [SerializeField] float speed = 0.0f;
    public float timeBeforeNextGust = 1.0f;

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
        ChangeTemperature(speed * Time.deltaTime);
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
        temperature += amount;
        float lerpVal = Mathf.Lerp(0.0f, 1.0f, temperature / 20.0f);
        
        Color c = ext.color;
        c.a = lerpVal;
        ext.color = c;

        thermometer.value = lerpVal;
    }

    public static void UpdateSpeed()
    {
        int res = 0;
        foreach (Piece pi in Array.FindAll(instance.pieces, p => !p.alive))
            --res;

        instance.speed = res;
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
