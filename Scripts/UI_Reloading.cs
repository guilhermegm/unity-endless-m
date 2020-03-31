using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Reloading : MonoBehaviour
{
    public Slider slider;
    public Image fill;

    public void setMax(float maxHealth)
    {
        slider.maxValue = maxHealth;
    }

    public void setValue(float health)
    {
        slider.value = health;
    }
}
