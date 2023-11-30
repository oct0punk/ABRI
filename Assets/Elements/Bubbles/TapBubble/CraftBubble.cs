using UnityEngine;

public class CraftBubble : MonoBehaviour
{
    public RawMaterial material;
    [HideInInspector] public Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
}
