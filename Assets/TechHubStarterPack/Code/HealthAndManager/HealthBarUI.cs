using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TechHub.Health;

namespace TechHub.Health
{
    public class HealthBarUI : MonoBehaviour
    {
        // The Health component instance to read from (assign in Inspector)
        public Health referencedHealth;

        // The UI slider to update (assign in Inspector)
        public Slider healthSliderUI;

        void Start()
        {
            if (referencedHealth == null || healthSliderUI == null)
            {
                Debug.LogWarning("HealthBarUI: referencedHealth or healthSliderUI not assigned.");
                return;
            }

            healthSliderUI.maxValue = referencedHealth.GetMaxHealth();
            healthSliderUI.value = referencedHealth.GetCurrentHealth();
        }

        void Update()
        {
            if (referencedHealth == null || healthSliderUI == null) return;
            healthSliderUI.value = referencedHealth.GetCurrentHealth();
        }
    }
}