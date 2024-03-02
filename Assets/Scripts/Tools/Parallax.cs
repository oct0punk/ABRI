using UnityEngine;

public class Parallax : MonoBehaviour
{
    [Min(0)] public int speed;
    Transform cam;
    Material mat;
    Vector2 offset;
    Vector2 lastCamPos;
    Vector2 camDelta;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
        lastCamPos = cam.transform.position;
        mat = GetComponentInChildren<MeshRenderer>().material;

        float ratio = 1.0f * Screen.width / Screen.height;
        transform.localScale = new Vector3(ratio * .2f, .2f, .2f);
        mat.SetTextureScale("_MainTex", new Vector2(ratio, 1.0f));
    }

    // Update is called once per frame
    void Update()
    {
        camDelta = (Vector2)cam.transform.position - lastCamPos;
        offset -= camDelta * speed * Time.deltaTime;
        mat.SetTextureOffset("_MainTex", offset);
        lastCamPos = cam.transform.position;
    }
}
