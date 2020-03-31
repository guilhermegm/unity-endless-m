using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public Monster monster;
    public int waves;
    public int monstersPerWave;
    public float startTimeBtwWaves;
    private float timeBtwWaves;
    private int wavesSpawned = 0;

    private void Update()
    {
        if (wavesSpawned == waves)
            return;

        timeBtwWaves -= Time.deltaTime;

        if (timeBtwWaves > 0)
            return;

        SpawnWave();
        timeBtwWaves = startTimeBtwWaves;
    }

    public void StartSpawn()
    {
        wavesSpawned = 0;
        SpawnWave();
        timeBtwWaves = startTimeBtwWaves;
    }

    private void SpawnWave()
    {
        wavesSpawned++;
        Debug.Log(GetInstanceID() + " - " + monster);
        for (int i = 0; i < monstersPerWave; i++)
        {
            Invoke("SpawnMonster", Random.Range(0, 3f));
        }
    }

    private void SpawnMonster()
    {
        Bounds bounds = GetComponent<Renderer>().bounds;
        float sizeX = bounds.size.x / 2;
        float sizeY = bounds.size.y / 2;

        Vector3 monsterPosition = new Vector3(Random.Range(-sizeX, sizeX) + transform.position.x, Random.Range(-sizeY, sizeY) + transform.position.y, 0f);
        GameObject monsterGO = Instantiate(monster.gameObject, monsterPosition, Quaternion.identity);

        GameManager gameManager = GameObject.FindObjectOfType<GameManager>();

        gameManager.waveManager.AddMonsterSpawned(monsterGO);
    }
}
