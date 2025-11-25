using UnityEngine;
using UnityEngine.UI;
public class TowerHealthManager : MonoBehaviour
{
  public Image HealthBar;
  public float HealthAmount = 100f;
  
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Update()
    {
      if (HealthAmount <= 0)
        {
            Application.LoadLevel(Application.loadedLevel);
        }
        
        if (Input.GetKeyDown(KeyCode.Return))
        {
            TakeDamage(20);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Heal(5);
        }
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        TakeDamage(20);
    }

    // Update is called once per frame

    public void TakeDamage (float damage)
    {
        HealthAmount -= damage;
        HealthBar.fillAmount = HealthAmount / 100f;

    }

    public void Heal(float healingAmount)
    {
        HealthAmount += healingAmount;
        HealthAmount = Mathf.Clamp(HealthAmount, 0, 100);

        HealthBar.fillAmount = HealthAmount / 100f;
    }
}
