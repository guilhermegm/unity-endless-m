using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MonsterGroup : ScriptableObject
{
    public Monster monster;
    public int quantity;
    public int waves;
    public float startTimeBtwWaves;
}
