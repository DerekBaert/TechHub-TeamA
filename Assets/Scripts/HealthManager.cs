using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HealthManager : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 3;
    private int _currentHealth;

    [Header("UI References")]
    [SerializeField] private List<Image> heartImages; // Drag your Heart UI Images here in order
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart; // Optional: if you have a "dead" heart sprite

    void Start()
    {
        _currentHealth = maxHealth;
        UpdateHeartUI();
    }

    // This is the method the Trash "Hander" script calls via SendMessage
    public void TakeDamage(int amount)
    {
        if (LevelManager.instance != null && LevelManager.instance.isGameOver) return;

        _currentHealth -= amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, maxHealth);

        UpdateHeartUI();

        if (_currentHealth <= 0)
        {
            GameOver();
        }
    }

    private void UpdateHeartUI()
    {
        for (int i = 0; i < heartImages.Count; i++)
        {
            if (i < _currentHealth)
            {
                heartImages[i].enabled = true; // Show heart
                if (fullHeart != null) heartImages[i].sprite = fullHeart;
            }
            else
            {
                // Either disable the image or switch to an empty heart sprite
                if (emptyHeart != null)
                {
                    heartImages[i].sprite = emptyHeart;
                }
                else
                {
                    heartImages[i].enabled = false; 
                }
            }
        }
    }

    private void GameOver()
    {
        Debug.Log("Game Over!");
        if (LevelManager.instance != null)
        {
            LevelManager.instance.isGameOver = true;
            // You can also trigger your PauseMenu's game over screen here
        }
    }
}