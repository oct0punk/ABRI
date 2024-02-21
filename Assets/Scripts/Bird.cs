using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    [SerializeField] GameObject tap;
    [SerializeField] Material fsShader;
    float timer;

    private void Awake()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, LayerMask.GetMask("Platform"));
        if (hit)
        {
            transform.SetParent(hit.transform);
        }
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
        yield return new WaitForSeconds(3);
        fsShader.SetFloat("_Alpha", 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        timer = Random.Range(5.0f, 10.0f);
        if (collision.GetComponentInParent<Lumberjack>() != null)
        {
            enabled = true;
            MakeNoise();
            tap.SetActive(true);
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

    public void OnGuts()
    {
        if (transform.position.x > Lumberjack.Instance.transform.position.x + 15.0f && !enabled)
            CameraManager.Instance.EmitFeathers();
    }
}
