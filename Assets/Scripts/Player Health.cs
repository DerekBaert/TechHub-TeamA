using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth;
    public HealthBar healthBar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;    
    }

    // Update is called once per frame
    private void Update()
    {
        healthBar.UpdateHealthBar(currentHealth);
    }

    public void TakeDamage(float damage)
    {
        //Debug.Log("Took damage");
        currentHealth -= damage;
    }
}
