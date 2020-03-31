using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class SpellCombo
{
    public Spell spellPrefab;
    public float minTimeToNextSpell;
    public float randomTimeToNextSpell;
    public float castDuration;
    public float radiusOffset;
    public SpellHandler.CastType castType;
}

public class SpellHandler : MonoBehaviour
{
    public SpellCombo[] spellCombosBase;

    [HideInInspector] public bool canCast;
    [HideInInspector] public bool isCasting;
    private int index;
    private float rechargingTime;
    private SpellCombo[] spellCombos;

    public enum CastType { Target, Cross, Star };

    private void Awake()
    {
        ShuffleSpellCombos();
    }

    private void Update()
    {
        if (canCast)
            return;

        if (isCasting)
            return;

        if (rechargingTime > 0)
        {
            rechargingTime -= Time.deltaTime;
            return;
        }
        
        canCast = true;
        isCasting = false;
    }

    public void Cast(Monster monster, Transform target)
    {
        if (!canCast)
            return;

        if (spellCombos[index].castType == CastType.Cross)
        {
            CastCross(monster);
            return;
        }

        if (spellCombos[index].castType == CastType.Star)
        {
            CastStar(monster);
            return;
        }

        Vector3 direction = (target.position - monster.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Spell spell = Instantiate(spellCombos[index].spellPrefab, monster.transform.position, lookRotation);

        spell.Init(LayerMask.GetMask("MonsterProjectil"), LayerMask.GetMask("Player"));
        spell.Cast(target);

        canCast = false;
        isCasting = true;
        Invoke("SetNextSpell", spellCombos[index].castDuration);
    }

    public void CastCross(Monster monster)
    {
        float[] angles = { 0, 90, 180, 270 };
        Vector2[] forces = { Vector2.right, Vector2.up, Vector2.left, Vector2.down };

        CastOnCircle(monster.transform.position, angles, forces);

        canCast = false;
        isCasting = true;
        Invoke("SetNextSpell", spellCombos[index].castDuration);
    }

    public void CastStar(Monster monster)
    {
        float[] angles = { 0, 45, 90, 135, 180, 225, 270, 315 };
        Vector2[] forces = {
            Vector2.right,
            Vector2.right + Vector2.up,
            Vector2.up,
            Vector2.up + Vector2.left,
            Vector2.left,
            Vector2.left + Vector2.down,
            Vector2.down,
            Vector2.down + Vector2.right
        };

        CastOnCircle(monster.transform.position, angles, forces);

        canCast = false;
        isCasting = true;
        Invoke("SetNextSpell", spellCombos[index].castDuration);
    }

    private void CastOnCircle(Vector3 position, float[] angles, Vector2[] forces)
    {
        for (int i = 0; i < angles.Length; i++)
        {
            Spell spell = InitSpell();
            spell.transform.position = position + CalcPositionOnCircle(angles[i]) * spellCombos[index].radiusOffset;
            spell.Cast(forces[i]);
        }
    }

    private Vector3 CalcPositionOnCircle(float angle)
    {
        return new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0f);
    }

    private Spell InitSpell()
    {
        Spell spell = Instantiate(spellCombos[0].spellPrefab);
        spell.Init(LayerMask.GetMask("MonsterProjectil"), LayerMask.GetMask("Player"));

        return spell;
    }

    private void SetNextSpell()
    {
        rechargingTime = spellCombos[index].minTimeToNextSpell + Random.Range(0f, spellCombos[index].randomTimeToNextSpell);
        canCast = false;
        isCasting = false;
        index++;

        if (index >= spellCombos.Length)
        {
            ShuffleSpellCombos();
            index = 0;
        }
    }

    private void ShuffleSpellCombos()
    {
        Random rnd = new Random();
        spellCombos = spellCombosBase.OrderBy(c => Random.Range(0f, 1000f)).ToArray();
    }
}
