using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Loot
{
    public Item item;
    public float chance;
    public int minQuantity;
    public int maxQuantity;

    public int Quantity { get; set; }
}

[CreateAssetMenu]
public class LootTable : ScriptableObject
{
    public Loot[] loots;

    public ArrayList Drops()
    {
        ArrayList dropped = new ArrayList();
        float cumProb = 0f;
        float currentProb = Random.Range(0f, 100f);

        foreach (Loot loot in loots)
        {
            cumProb += loot.chance;

            if (currentProb <= cumProb)
            {
                loot.Quantity = RandomQuantity(loot);
                dropped.Add(loot);
            }
        }

        return dropped;
    }

    public int RandomQuantity(Loot loot)
    {
        return Random.Range(loot.minQuantity, loot.maxQuantity);
    }
}
