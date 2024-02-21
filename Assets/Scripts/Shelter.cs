using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using UnityEditor;
using System;

[SelectionBase]
public class Shelter : MonoBehaviour
{
    public static Shelter instance;
    [SerializeField] SpriteRenderer ext;
    [SerializeField] new Light2D light;
    public CinemachineVirtualCamera cam;
    [Space]
    [Header("Temperature")]
    [SerializeField] float temperature = 20.0f;
    [SerializeField] float maxTemperature = 25.0f;
    [Header("Storm")]
    [SerializeField] float speed = .5f;
    [SerializeField] float timeBeforeNextGust = 1.0f;
    [SerializeField] int push = 0;
    Storm storm;
    [Space]
    [SerializeField] Slider thermometer;
    [Space]
    public AudioSource brokenWind;
    public Piece[] pieces;


    [ContextMenu("Init")]
    void Awake()
    {
        instance = this;
        storm = GetComponent<Storm>();
        pieces = GetComponentsInChildren<Piece>();
    }



    private void Update()
    {
        ChangeTemperature(push * Time.deltaTime * speed);

        timeBeforeNextGust -= Time.deltaTime;
        if (timeBeforeNextGust < 0.0f)
        {
            FindObjectOfType<Bird>().OnGuts();
            CameraManager.Instance.EmitFeathers();
            Lumberjack.Instance.Message("Ouf, quelle rafale! J'espère que mon abri l'a encaissé...");
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
            GameManager.instance.ChangeState(GameState.GameOver);
            enabled = false;
            return;
        }


        thermometer.value = Mathf.Lerp(0.0f, 1.0f, temperature / maxTemperature);
    }
    public void UpdateSpeed(int amount)
    {
        push = Mathf.Min(push, 0) + amount;
        if (Array.TrueForAll(pieces, p => p.build))
        {
            if (brokenWind.isPlaying)
            brokenWind.Stop();
            push = 2;
        }
        else
        {
            if (!brokenWind.isPlaying)
            brokenWind.Play();
        }
    }


    void OnEnter()
    {
        // Visibility
        ext.enabled = false;
        light.enabled = true;
    }
    void OnExit()
    {
        ext.enabled = true;
        light.enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Le bucheron ENTRE dans la cabane
        Lumberjack lum = collision.GetComponentInParent<Lumberjack>();
        if (lum != null)
        {
            GameManager.instance.ChangeState(GameState.Indoor);
            OnEnter();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Le bucheron SORT
        Lumberjack lum = collision.GetComponentInParent<Lumberjack>();
        if (lum != null)
        {
            GameManager.instance.ChangeState(GameState.Explore);
            OnExit();
        }
    }
}