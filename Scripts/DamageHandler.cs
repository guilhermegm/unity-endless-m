using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DamageHandler
{
    public enum Element
    {
        Wind,
        Earth,
        Water,
        Fire,
        Neutral,
        Holy,
        Shadow,
        Ghost,
        Undead,
        Poison
    }

    public static float WeaponVsMonster(Weapon weapon, EntityStatus entityStatus)
    {
        float elementMod;
        elementsMod.TryGetValue(weapon.weaponStats.element.ToString() + entityStatus.element.ToString(), out elementMod);

        float attack = Random.Range(weapon.GetMinAttack(), weapon.GetMaxAttack()) * elementMod;
        float defense = entityStatus.baseDeffense;
        float damage = attack - defense;

        return damage > 0 ? damage : 0;
    }

    public static float PlayerVsMonster(Player player, Spell spell)
    {
        float elementMod;
        elementsMod.TryGetValue(spell.weaponStats.element.ToString() + player.entityStatus.element.ToString(), out elementMod);

        float attack = Random.Range(spell.weaponStats.minAttack, spell.weaponStats.maxAttack) * elementMod;
        float defense = player.entityStatus.baseDeffense;
        float damage = attack - defense;

        return damage > 0 ? damage : 0;
    }

    private static IDictionary<string, float> elementsMod = new Dictionary<string, float>()
    {
        {Element.Wind.ToString() + Element.Wind.ToString(), 0.25f},
        {Element.Wind.ToString() + Element.Earth.ToString(), 2f},
        {Element.Wind.ToString() + Element.Water.ToString(), 0.5f},
        {Element.Wind.ToString() + Element.Fire.ToString(), 1f},
        {Element.Wind.ToString() + Element.Neutral.ToString(), 1f},
        {Element.Wind.ToString() + Element.Holy.ToString(), 1f},
        {Element.Wind.ToString() + Element.Shadow.ToString(), 1f},
        {Element.Wind.ToString() + Element.Ghost.ToString(), 1f},
        {Element.Wind.ToString() + Element.Undead.ToString(), 0.5f},
        {Element.Wind.ToString() + Element.Poison.ToString(), 1.25f},
        {Element.Earth.ToString() + Element.Wind.ToString(), 0.5f},
        {Element.Earth.ToString() + Element.Earth.ToString(), 0.25f},
        {Element.Earth.ToString() + Element.Water.ToString(), 1f},
        {Element.Earth.ToString() + Element.Fire.ToString(), 0.5f},
        {Element.Earth.ToString() + Element.Neutral.ToString(), 1f},
        {Element.Earth.ToString() + Element.Holy.ToString(), 1f},
        {Element.Earth.ToString() + Element.Shadow.ToString(), 1f},
        {Element.Earth.ToString() + Element.Ghost.ToString(), 1f},
        {Element.Earth.ToString() + Element.Undead.ToString(), 0.5f},
        {Element.Earth.ToString() + Element.Poison.ToString(), 1f},
        {Element.Water.ToString() + Element.Wind.ToString(), 1f},
        {Element.Water.ToString() + Element.Earth.ToString(), 1f},
        {Element.Water.ToString() + Element.Water.ToString(), 1f},
        {Element.Water.ToString() + Element.Fire.ToString(), 1f},
        {Element.Water.ToString() + Element.Neutral.ToString(), 1f},
        {Element.Water.ToString() + Element.Holy.ToString(), 1f},
        {Element.Water.ToString() + Element.Shadow.ToString(), 1f},
        {Element.Water.ToString() + Element.Ghost.ToString(), 1f},
        {Element.Water.ToString() + Element.Undead.ToString(), 1f},
        {Element.Water.ToString() + Element.Poison.ToString(), 1f},
        {Element.Fire.ToString() + Element.Wind.ToString(), 1f},
        {Element.Fire.ToString() + Element.Earth.ToString(), 1f},
        {Element.Fire.ToString() + Element.Water.ToString(), 1f},
        {Element.Fire.ToString() + Element.Fire.ToString(), 1f},
        {Element.Fire.ToString() + Element.Neutral.ToString(), 1f},
        {Element.Fire.ToString() + Element.Holy.ToString(), 1f},
        {Element.Fire.ToString() + Element.Shadow.ToString(), 1f},
        {Element.Fire.ToString() + Element.Ghost.ToString(), 1f},
        {Element.Fire.ToString() + Element.Undead.ToString(), 1f},
        {Element.Fire.ToString() + Element.Poison.ToString(), 1f},
        {Element.Neutral.ToString() + Element.Wind.ToString(), 1f},
        {Element.Neutral.ToString() + Element.Earth.ToString(), 1f},
        {Element.Neutral.ToString() + Element.Water.ToString(), 1f},
        {Element.Neutral.ToString() + Element.Fire.ToString(), 1f},
        {Element.Neutral.ToString() + Element.Neutral.ToString(), 1f},
        {Element.Neutral.ToString() + Element.Holy.ToString(), 1f},
        {Element.Neutral.ToString() + Element.Shadow.ToString(), 1f},
        {Element.Neutral.ToString() + Element.Ghost.ToString(), 1f},
        {Element.Neutral.ToString() + Element.Undead.ToString(), 1f},
        {Element.Neutral.ToString() + Element.Poison.ToString(), 1f},
        {Element.Holy.ToString() + Element.Wind.ToString(), 1f},
        {Element.Holy.ToString() + Element.Earth.ToString(), 1f},
        {Element.Holy.ToString() + Element.Water.ToString(), 1f},
        {Element.Holy.ToString() + Element.Fire.ToString(), 1f},
        {Element.Holy.ToString() + Element.Neutral.ToString(), 1f},
        {Element.Holy.ToString() + Element.Holy.ToString(), 1f},
        {Element.Holy.ToString() + Element.Shadow.ToString(), 1f},
        {Element.Holy.ToString() + Element.Ghost.ToString(), 1f},
        {Element.Holy.ToString() + Element.Undead.ToString(), 1f},
        {Element.Holy.ToString() + Element.Poison.ToString(), 1f},
        {Element.Shadow.ToString() + Element.Wind.ToString(), 1f},
        {Element.Shadow.ToString() + Element.Earth.ToString(), 1f},
        {Element.Shadow.ToString() + Element.Water.ToString(), 1f},
        {Element.Shadow.ToString() + Element.Fire.ToString(), 1f},
        {Element.Shadow.ToString() + Element.Neutral.ToString(), 1f},
        {Element.Shadow.ToString() + Element.Holy.ToString(), 1f},
        {Element.Shadow.ToString() + Element.Shadow.ToString(), 1f},
        {Element.Shadow.ToString() + Element.Ghost.ToString(), 1f},
        {Element.Shadow.ToString() + Element.Undead.ToString(), 1f},
        {Element.Shadow.ToString() + Element.Poison.ToString(), 1f},
        {Element.Ghost.ToString() + Element.Wind.ToString(), 1f},
        {Element.Ghost.ToString() + Element.Earth.ToString(), 1f},
        {Element.Ghost.ToString() + Element.Water.ToString(), 1f},
        {Element.Ghost.ToString() + Element.Fire.ToString(), 1f},
        {Element.Ghost.ToString() + Element.Neutral.ToString(), 1f},
        {Element.Ghost.ToString() + Element.Holy.ToString(), 1f},
        {Element.Ghost.ToString() + Element.Shadow.ToString(), 1f},
        {Element.Ghost.ToString() + Element.Ghost.ToString(), 1f},
        {Element.Ghost.ToString() + Element.Undead.ToString(), 1f},
        {Element.Ghost.ToString() + Element.Poison.ToString(), 1f},
        {Element.Undead.ToString() + Element.Wind.ToString(), 1f},
        {Element.Undead.ToString() + Element.Earth.ToString(), 1f},
        {Element.Undead.ToString() + Element.Water.ToString(), 1f},
        {Element.Undead.ToString() + Element.Fire.ToString(), 1f},
        {Element.Undead.ToString() + Element.Neutral.ToString(), 1f},
        {Element.Undead.ToString() + Element.Holy.ToString(), 1f},
        {Element.Undead.ToString() + Element.Shadow.ToString(), 1f},
        {Element.Undead.ToString() + Element.Ghost.ToString(), 1f},
        {Element.Undead.ToString() + Element.Undead.ToString(), 1f},
        {Element.Undead.ToString() + Element.Poison.ToString(), 1f},
        {Element.Poison.ToString() + Element.Wind.ToString(), 1f},
        {Element.Poison.ToString() + Element.Earth.ToString(), 1f},
        {Element.Poison.ToString() + Element.Water.ToString(), 1f},
        {Element.Poison.ToString() + Element.Fire.ToString(), 1f},
        {Element.Poison.ToString() + Element.Neutral.ToString(), 1f},
        {Element.Poison.ToString() + Element.Holy.ToString(), 1f},
        {Element.Poison.ToString() + Element.Shadow.ToString(), 1f},
        {Element.Poison.ToString() + Element.Ghost.ToString(), 1f},
        {Element.Poison.ToString() + Element.Undead.ToString(), 1f},
        {Element.Poison.ToString() + Element.Poison.ToString(), 1f}
    };
}
