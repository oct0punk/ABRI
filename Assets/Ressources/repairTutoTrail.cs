using UnityEngine;

public class repairTutoTrail : MonoBehaviour
{
    public RectTransform repairBubble;
    public Transform piece;
    public TrailRenderer trail;
    public GameObject tuto;
    public AnimationCurve curve;
    public float time = 0.0f;

    private void Update()
    {
        time += Time.deltaTime;
        if (time > 2.0f)
        {
            tuto.SetActive(true);
            time = -0.3f;
            transform.position = Camera.main.ScreenToWorldPoint(repairBubble.position);
        }
        else if (time > 1.5f)
        {
            tuto.SetActive(false);
            trail.emitting = false;
        }
        else if (time < 1.0f)
        {
            if (time > 0.0f)
            {
                transform.position = Vector3.Lerp(Camera.main.ScreenToWorldPoint(repairBubble.position), piece.position, curve.Evaluate(time));
                trail.emitting = true;
            }
        }
    }
}
