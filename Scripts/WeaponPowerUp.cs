using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPowerUp : MonoBehaviour
{
    [Range(0f, 1f)]
    public float bonusPercent;
    public int level;
    public int maxLevel;
    public int basePrice;

    public int GetPrice()
    {
        return basePrice * level + ((int)GetTotalBonusPercent() * basePrice * level);
    }

    public float GetTotalBonusPercent()
    {
        return bonusPercent * level;
    }

    public void Upgrade()
    {
        if (level >= maxLevel)
            return;

        level++;
    }
}
