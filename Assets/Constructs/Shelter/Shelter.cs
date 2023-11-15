using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering.Universal;

public class Shelter : MonoBehaviour
{
    [SerializeField] SpriteRenderer ext;
    [SerializeField] Light2D light;
    [SerializeField] CinemachineVirtualCamera cam;

    // Start is called before the first frame update
    void Start()
    {
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
