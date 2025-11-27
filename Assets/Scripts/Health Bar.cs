using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthBar;
    private float currentValue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthBar = GetComponent<Slider>();  
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = currentValue;
        Debug.Log(healthBar.value);
    }

    public void UpdateHealthBar(float value)
    {
       //Debug.Log("Health updated " + value);
        currentValue = value;
    }
}
