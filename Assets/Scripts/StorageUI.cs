using TMPro;
using UnityEngine;

public class StorageUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Tmp;
    float timer = .0f;
    Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0.0f)
            {
                enabled = false;
                transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    void Display(int amount, float time)
    {
        Tmp.text = amount.ToString();
        timer = time;
        enabled = true;
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void Add(int amount)
    {
        Display(amount, 3.0f);
        animator.SetTrigger("Collect");
    }

    public void Remove(int amount)
    {
        Display(amount, 3.0f);
        animator.SetTrigger("Consume");
    }

    public void Required(int amount)
    {
        Display(amount, 3.0f);
    }
    
}
