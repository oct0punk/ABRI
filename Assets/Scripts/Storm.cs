using System;
using UnityEngine;

public class Storm : MonoBehaviour
{
    [Range(1, 5)]
    public int wind;

    [SerializeField] Vector2 gutsFrequency;
    [SerializeField] float timeBeforeNextGust = 1.0f;
    Shelter shelter;

    [SerializeField] ParticleSystem[] fx;

    private void Awake()
    {
        shelter = GetComponent<Shelter>();
    }
    
    private void Update()
    {
        timeBeforeNextGust -= Time.deltaTime;
        if (timeBeforeNextGust < 0.0f)
        {
            enabled = false;
            if (GameManager.instance.gameState == GameState.Explore)
            {
                Lumberjack.Instance.Message("GutOutside", 1.0f, Guts);
            }
            else if (GameManager.instance.gameState == GameState.Indoor)
            {
                Lumberjack.Instance.Message("GutInside", .5f, Guts);
            }
        }
    }

    public void Guts()
    {
        Bird.SendClueToPlayer(2);
        timeBeforeNextGust = UnityEngine.Random.Range(gutsFrequency.x, gutsFrequency.y);
        enabled = true;
        Array.ForEach(fx, f => f.Play());
        CameraManager.Instance.CamShake();
        AudioManager.Instance.Play("Guts");

        if (GameManager.instance.gameState == GameState.End)
        {
            return;
        }
        

        foreach (Piece p in shelter.pieces)
        {
            p.Resist(wind);
        }

        if (GameManager.instance.gameState == GameState.Explore)
        {
            if (Array.TrueForAll(shelter.pieces, p => p.life < 2))
            {
                if (Array.TrueForAll(shelter.pieces, p => p.life == 1))
                    shelter.pieces[0].life = 3;
                else
                    Lumberjack.Instance.Message("ShelterCritical");
            }
        }
        else if (GameManager.instance.gameState == GameState.Indoor)
        {
            if (Array.TrueForAll(shelter.pieces, p => p.build))
            {
                shelter.restored = true;
            }
            else
            {
                Lumberjack.Instance.Message("OnHoleInside");
                shelter.restored = false;
            }
        }

    }
}
