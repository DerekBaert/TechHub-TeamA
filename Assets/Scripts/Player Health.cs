using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth;
    public HealthBar healthBar;
    public UIManager uIManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;    
    }

    // Update is called once per frame
    private void Update()
    {
        healthBar.UpdateHealthBar(currentHealth);
        if (currentHealth == 0)
        {
            PlayerDied();
        }
    }

    private void PlayerDied()
    {
        LevelManager.instance.GameOver();
        gameObject.SetActive(false);
    }

    public void TakeDamage(float damage)
    {
        //Debug.Log("Took damage");
        currentHealth -= damage;
    }
}
