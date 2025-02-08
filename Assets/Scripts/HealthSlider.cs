using System;
using UnityEngine;
using UnityEngine.UI;

namespace KrazyKatGames
{
    public class HealthSlider : MonoBehaviour
    {
        [SerializeField]
        private Slider _slider;

        public void SetMaxHealth(int maxHealth)
        {
            if (_slider)
                _slider.maxValue = maxHealth;
        }

        public void SetHealth(int health)
        {
            if (_slider)
                _slider.value = health;
        }
    }
}