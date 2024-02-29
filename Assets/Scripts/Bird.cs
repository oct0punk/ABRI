using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour, IFix
{
    [SerializeField] GameObject tap;
    [SerializeField] Material fsShader;
    float timer;
    bool logAudio;
    bool first = true;
    static Bird instance;

    private void Awake()
    {
        IFix fix = this;
        fix.Fix(transform);
    }

    private void Update()
    {
        Vector2 viewportPos = Camera.main.WorldToViewportPoint(transform.position);
        viewportPos.x = Mathf.Clamp01(viewportPos.x);
        viewportPos.y = Mathf.Clamp01(viewportPos.y);
        fsShader.SetVector("_Pos", viewportPos);
        
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            StartCoroutine(MakeNoise());
        }
    }

    IEnumerator MakeNoise()
    {
        AudioManager.Instance.Play("Bird").panStereo = transform.position.x - Lumberjack.Instance.transform.position.x;
        timer = Random.Range(5.0f, 10.0f);
        fsShader.SetFloat("_Alpha", 1);
        yield return new WaitForSeconds(1.4f);
        fsShader.SetFloat("_Alpha", 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        timer = Random.Range(5.0f, 10.0f);
        if (collision.GetComponentInParent<Lumberjack>() != null)
        {
            enabled = true;
            StartCoroutine(MakeNoise());
            tap.SetActive(true);
            if (!logAudio)
            {
                if (first) {
                    Lumberjack.Instance.Message("L'oiseau est audible. Il n'est pas loin.");
                    first = false;
                }
                else {
                    Lumberjack.Instance.Message("L'oiseau est de nouveau audible. J'ai retrouvé sa trace.");
                }

                logAudio = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<Lumberjack>() != null)
        {
            enabled = false;
            tap.SetActive(false);
        }
    }


    public void Catch()
    {
        GameManager.instance.ChangeState(GameState.End);
    }

    public static void SendClueToPlayer(int time = 0, int chance = 0)
    {
        if (Random.Range(0, chance) != 0) return;
        if (instance == null) instance = FindObjectOfType<Bird>();
        instance.Invoke("SendClue", time);
    }

    void SendClue()
    {
        if (Lumberjack.Instance.transform.position.x < transform.position.x - 15.0f)
            CameraManager.Instance.EmitFeathers();
        else if (Lumberjack.Instance.transform.position.x > transform.position.x + 15.0f)
        {
            Lumberjack.Instance.Message("Aucun signe de l'oiseau dans cette direction. Je devrais faire demi-tour");
            logAudio = false;
        }
    }
}
