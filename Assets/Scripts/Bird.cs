using System.Collections;
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
        timer = UnityEngine.Random.Range(5.0f, 10.0f);
        if (collision.GetComponentInParent<Lumberjack>() != null)
        {
            enabled = true;
            StartCoroutine(MakeNoise());
            tap.SetActive(true);
            if (!logAudio)
            {
                if (first)
                {
                    Lumberjack.Instance.Message("OnBirdEnter");
                    first = false;
                }
                else
                {
                    Lumberjack.Instance.Message("OnBirdEnterAgain");
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
        Lumberjack.Instance.AutoMoveTo(transform.position, OnCatch, () => transform.position);
    }

    void OnCatch()
    {
        Lumberjack.Instance.OnCatch();
        AudioManager.Instance.Play("OnCatch");
        GameManager.instance.ChangeState(GameState.End);
        Destroy(gameObject);
    }

    
    public static void SendClueToPlayer(int time = 0, int chance = 0)
    {
        if (Lumberjack.hasCaught) return;
        if (Random.Range(0, chance) != 0) return;
        if (instance == null) instance = FindObjectOfType<Bird>();
        instance.Invoke(nameof(SendClue), time);
    }

    void SendClue()
    {
        if (Lumberjack.Instance.transform.position.x < transform.position.x - 15.0f)
            CameraManager.Instance.EmitFeathers();
        else if (Lumberjack.Instance.transform.position.x > transform.position.x + 15.0f)
        {
            Lumberjack.Instance.Message("OnBirdExit", 0.0f, null, true);
            logAudio = false;
        }
    }
}
