using System;
using UnityEngine;

public enum CullingAction
{
    Deactivate,
    Hide,
    HideChildrens
}

public class OcclusionCulling2D : MonoBehaviour
{
    [System.Serializable]
    public class ObjectSettings
    {
        [HideInInspector] public string title;
        public GameObject theGameObject;
        public CullingAction cullingAction;

        public Vector2 size = Vector2.one;
        public Vector2 offset = Vector2.zero;
        public bool multiplySizeByTransformScale = true;

        public Vector2 sized { get; set; }
        public Vector2 center { get; set; }
        public Vector2 TopRight { get; set; }
        public Vector2 TopLeft { get; set; }
        public Vector2 BottomLeft { get; set; }
        public Vector2 BottomRight { get; set; }
        public float right { get; set; }
        public float left { get; set; }
        public float top { get; set; }
        public float bottom { get; set; }

        public Color DrawColor = Color.white;
        public bool showBorders = true;
    }

    public ObjectSettings[] objectSettings = new ObjectSettings[1];

    private new Camera camera;
    private float cameraHalfWidth;

    public float updateRateInSeconds = 0.1f;

    private float timer;

    void Awake()
    {
        camera = GetComponent<Camera>();

        cameraHalfWidth = camera.orthographicSize * ((float)Screen.width / (float)Screen.height) * 10;

        foreach (ObjectSettings o in objectSettings)
        {
            o.sized = o.size * (o.multiplySizeByTransformScale ? new Vector2(Mathf.Abs(o.theGameObject.transform.localScale.x), Mathf.Abs(o.theGameObject.transform.localScale.y)) : Vector2.one);
            o.center = (Vector2)o.theGameObject.transform.position + o.offset;

            o.TopRight = new Vector2(o.center.x + o.sized.x, o.center.y + o.sized.y);
            o.TopLeft = new Vector2(o.center.x - o.sized.x, o.center.y + o.sized.y);
            o.BottomLeft = new Vector2(o.center.x - o.sized.x, o.center.y - o.sized.y);
            o.BottomRight = new Vector2(o.center.x + o.sized.x, o.center.y - o.sized.y);

            o.right = o.center.x + o.sized.x;
            o.left = o.center.x - o.sized.x;
            o.top = o.center.y + o.sized.y;
            o.bottom = o.center.y - o.sized.y;
        }
    }

    private void OnDrawGizmosSelected()
    {
        foreach (ObjectSettings o in objectSettings)
        {
            if (o.theGameObject)
            {
                o.title = o.theGameObject.name;

                if (o.theGameObject.GetComponent<Renderer>() != null) 
                {
                    o.offset = o.theGameObject.GetComponent<Renderer>().bounds.center - o.theGameObject.transform.position;
                    o.size = o.theGameObject.GetComponent<Renderer>().bounds.size / 2;
                }

                if (o.showBorders)
                {
                    o.BottomRight = new Vector2(o.center.x + o.sized.x, o.center.y - o.sized.y);
                    Gizmos.color = o.DrawColor;
                    Gizmos.DrawWireCube(o.theGameObject.transform.position + (Vector3)o.offset, o.size * 2);
                    

                    // Gizmos.DrawLine(o.TopRight, o.TopLeft);
                    // Gizmos.DrawLine(o.TopLeft, o.BottomLeft);
                    // Gizmos.DrawLine(o.BottomLeft, o.BottomRight);
                    // Gizmos.DrawLine(o.BottomRight, o.TopRight);
                }
            }
        }
    }


    void FixedUpdate()
    {
        timer += Time.deltaTime;
        if (timer > updateRateInSeconds) timer = 0;
        else return;

        float cameraRight = camera.transform.position.x + cameraHalfWidth;
        float cameraLeft = camera.transform.position.x - cameraHalfWidth;
        float cameraTop = camera.transform.position.y + camera.orthographicSize;
        float cameraBottom = camera.transform.position.y - camera.orthographicSize;

        foreach (ObjectSettings o in objectSettings)
        {
            if (o.theGameObject)
            {
                bool IsObjectVisibleInCastingCamera = o.right > cameraLeft & o.left < cameraRight & // check horizontal
                                                      o.top > cameraBottom & o.bottom < cameraTop; // check vertical
                
                switch (o.cullingAction)
                {
                    case CullingAction.Hide:
                        Renderer rend = o.theGameObject.GetComponent<Renderer>();
                        if (rend)
                            rend.enabled = IsObjectVisibleInCastingCamera;
                        else
                            Debug.LogWarning(o.title + " : No renderer detected.");
                        break;
                    case CullingAction.Deactivate:
                        o.theGameObject.SetActive(IsObjectVisibleInCastingCamera);
                        break;
                    case CullingAction.HideChildrens:
                        Array.ForEach<Renderer>(o.theGameObject.GetComponentsInChildren<Renderer>(), r => r.enabled = IsObjectVisibleInCastingCamera);
                        Array.ForEach<Animator>(o.theGameObject.GetComponentsInChildren<Animator>(), r => r.enabled = IsObjectVisibleInCastingCamera);
                        break;
                }
            }
        }
    }
}