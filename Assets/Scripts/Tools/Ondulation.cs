using UnityEngine;

public class Ondulation : MonoBehaviour
{
    public float amplitude = .4f;
    Vector3 worldPos;

    public void SetWorldPos()
    {
        worldPos = transform.position;
    }

    private void Start()
    {
        SetWorldPos();
    }
    private void Update()
    {
        transform.position = worldPos + new Vector3(
            Mathf.PerlinNoise1D(worldPos.x + Time.timeSinceLevelLoad) - .5f,
            Mathf.PerlinNoise1D(worldPos.y + Time.timeSinceLevelLoad) - .5f,
            0) * amplitude; ;
            
    }
}
