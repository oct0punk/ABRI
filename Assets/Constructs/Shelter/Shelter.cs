using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering.Universal;

[SelectionBase]
public class Shelter : MonoBehaviour
{
    [SerializeField] SpriteRenderer ext;
    [SerializeField] Light2D light;
    [SerializeField] CinemachineVirtualCamera cam;

    [SerializeField] float temperature = 20.0f;
    [SerializeField] float speed = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
    }

    private void Update()
    {
        ChangeTemperature(speed * Time.deltaTime);
    }

    void ChangeTemperature(float amount)
    {
        temperature += amount;
        Color c = ext.color;
        c.a = Mathf.Lerp(0.0f, 1.0f, temperature / 20.0f);
        ext.color = c;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<Lumberjack>() != null) {
            ext.enabled = false;
            light.enabled = true;
            CameraManager.Possess(cam);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Lumberjack lum = collision.GetComponentInParent<Lumberjack>();
        if (lum != null)
        {
            ext.enabled = true;
            CameraManager.Possess(lum.cam);
            light.enabled = false;
        }
    }
}
