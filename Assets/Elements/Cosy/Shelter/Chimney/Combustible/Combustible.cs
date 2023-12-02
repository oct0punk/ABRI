using UnityEngine;


public class Combustible : MonoBehaviour
{
    Material mat;
    [Range(0.0f, 1.0f)]
    public float consuming;// { get; private set; }
    public int intensity = 2;
    bool active = false;

    private void Start()
    {

        mat = GetComponent<SpriteRenderer>().material;
        mat.SetFloat("_consuming", consuming);

        if (consuming < 1.0f)
        {
            active = true;
            Shelter.UpdateSpeed(intensity);
        }
    }
    private void Update()
    {
        if (!active) return;

        consuming = Mathf.Clamp01(consuming + Time.deltaTime * .01f);
        mat.SetFloat("_consuming", consuming);
        if (consuming >= 1.0f)
            Deactivate();
    }


    public void Activate()
    {
        if (!active)
            Shelter.UpdateSpeed(intensity);
        consuming = 0.0f;
        active = true;
    }
    public void Deactivate()
    {
        if (!active) return;
        
        Shelter.UpdateSpeed(-intensity);        
        active = false;
    }

}
