using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public List<MonsterSpawner> monsterSpawners;
    public WaveLevel[] waveLevels;
    public int level;
    public int MaxLevel { get { return maxLevel; } }

    private List<GameObject> monstersSpawned;
    private int totalMonstersSpawned;
    private GameManager gameManager;
    private int maxLevel;

    public void Init(GameManager gameManager)
    {
        this.gameManager = gameManager;
        maxLevel = level;
    }

    public void StartWave()
    {
        int waveLevel = (level - 1) % waveLevels.Length;
        int difficultMultiply = Mathf.CeilToInt(level / waveLevels.Length);

        monstersSpawned = new List<GameObject>();
        List<MonsterSpawner> monsterSpawnerFree = new List<MonsterSpawner>(monsterSpawners);

        foreach (MonsterGroup monsterGroup in waveLevels[waveLevel].monsterGroups)
        {
            if (monsterSpawnerFree.Count <= 0)
                return;

            int index = Random.Range(0, monsterSpawnerFree.Count);

            monsterSpawnerFree[index].monster = monsterGroup.monster;
            monsterSpawnerFree[index].waves = monsterGroup.waves;
            monsterSpawnerFree[index].monstersPerWave = monsterGroup.quantity;
            monsterSpawnerFree[index].startTimeBtwWaves = monsterGroup.startTimeBtwWaves;
            totalMonstersSpawned += monsterGroup.quantity * monsterGroup.waves;

            Debug.Log(monsterSpawnerFree[index].GetInstanceID() + " ... " + monsterGroup.monster);
            monsterSpawnerFree[index].StartSpawn();
            monsterSpawnerFree.RemoveAt(index);
        }

        gameManager.uiWaveTimer.text.SetText(totalMonstersSpawned.ToString("0"));
    }

    public void AddMonsterSpawned(GameObject monster)
    {
        monstersSpawned.Add(monster.gameObject);
    }

    public void OnMonsterSpawnedDeath(GameObject monster)
    {
        if (monstersSpawned == null || monstersSpawned.Count == 0)
            return;

        if (monstersSpawned.Contains(monster))
        {
            monstersSpawned.Remove(monster);
            totalMonstersSpawned--;

            gameManager.uiWaveTimer.text.SetText(totalMonstersSpawned.ToString("0"));
        }

        if (totalMonstersSpawned <= 0)
        {
            Debug.Log("All Dead");
            maxLevel++;
            SetNextLevel();
            gameManager.WaveDone();
        }
    }
    

    public void SetNextLevel()
    {
        if (level >= maxLevel)
            return;

        level++;
    }

    public void SetPreviousLevel()
    {
        if (level <= 1)
            return;

        level--;
    }
}

