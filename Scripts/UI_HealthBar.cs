using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public TextMeshProUGUI textMeshPro;

    public void setMax(float maxHealth)
    {
        slider.maxValue = maxHealth;
        updateGfx();
    }

    public void setValue(float health)
    {
        slider.value = health;
        updateGfx();
    }

    private void updateGfx()
    {
        fill.color = gradient.Evaluate(slider.normalizedValue);
        textMeshPro.SetText(slider.value.ToString("0") + "/" + slider.maxValue.ToString("0"));
    }
}
