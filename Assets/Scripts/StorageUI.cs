using TMPro;
using UnityEngine;

public class StorageUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Tmp;
    float timer = .0f;

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

    public void Display(int amount, float time)
    {
        Tmp.text = amount.ToString();
        timer = time;
        enabled = true;
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void Required(int amount)
    {

    }
}
