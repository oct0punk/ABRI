using UnityEngine;


public class Combustible : MonoBehaviour
{
    Material mat;    
    [SerializeField] [Range(0.0f,1.0f)] private float consuming; // 1.0f means that it is consumed
    bool active = false;


    private void Start()
    {
        mat = GetComponent<SpriteRenderer>().material;
        if (consuming < 1.0f)
            Activate();
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
        if (active) return;
        
        Shelter.UpdateSpeed(1);
        Reload();
    }
    public void Deactivate()
    {
        if (!active) return;
        
        Shelter.UpdateSpeed(-1);        
        active = false;
    }
    public void Reload()
    {
        active = true;
        consuming = 0.0f;
    }

}
