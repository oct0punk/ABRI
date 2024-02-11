using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using UnityEditor;
using System;

[SelectionBase]
public class Shelter : MonoBehaviour
{
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


    void Awake()
    {
        storm = GetComponent<Storm>();
        pieces = GetComponentsInChildren<Piece>();
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
    public void UpdateSpeed(int amount)
    {
        push += amount;
        if (Array.TrueForAll<Piece>(pieces, p => p.alive))
        {
            if (brokenWind.isPlaying)
            brokenWind.Stop();
        }
        else
        {
            if (!brokenWind.isPlaying)
            brokenWind.Play();
        }
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
        }
    }
    void OnExit()
    {
        ext.enabled = true;
        light.enabled = false;
    }
}