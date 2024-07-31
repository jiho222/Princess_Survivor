using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData; // 레벨마다 데이터가 필요하니까 배열
    public float levelTime;

    public GameObject boss1Prefab;
    public GameObject boss2Prefab;
    private bool boss1Spawned = false;
    private bool boss2Spawned = false;

    int level;
    float timer;

    void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
        levelTime = GameManager.instance.maxGameTime / spawnData.Length;
        // 최대 시간에 몬스터 데이터 크기로 나누어 자동으로 구간 시간 계산
        // 120초에 몬스터 6개면 20초마다 단계 업
    }
    
    void Update() // levelTime 에 맞게 spawnTime 에 맞게 Spawn
    {
        if (!GameManager.instance.isLive)
            return;
            
        timer += Time.deltaTime;
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / levelTime), spawnData.Length -1);

        if (timer > spawnData[level].spawnTime) {
            Spawn();
            timer = 0;
        }

        BossSpawn();
    }

    void Spawn()
    {
        GameObject enemy = GameManager.instance.pool.Get(0);
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position; // 0번째는 부모 오브젝트이므로 1부터 시작
        enemy.GetComponent<Enemy>().Init(spawnData[level]);
    }

    void BossSpawn()
    {
        // 5분(300초)일 때 boss1을 스폰, 단 한 번만
        if (GameManager.instance.gameTime >= 5f && !boss1Spawned)
        {
            Debug.Log("Boss1 Spawned");
            GameObject boss1 = Instantiate(boss1Prefab);
            
            boss1.transform.position = GameManager.instance.player.transform.position + Vector3.up * 8f;
            boss1Spawned = true; // Boss1이 이미 스폰되었음을 기록
        }

        // 10분(600초)일 때 boss2를 스폰, 단 한 번만
        if (GameManager.instance.gameTime >= 600f && !boss2Spawned)
        {
            GameObject boss2 = Instantiate(boss2Prefab);
            boss2.transform.position = GameManager.instance.player.transform.position + Vector3.up * 8f;
            boss2Spawned = true; // Boss2가 이미 스폰되었음을 기록
        }
    }
}

[System.Serializable] // 직렬화
public class SpawnData
{
    public int spriteType;
    public float spawnTime;
    public int health;
    public float speed;
}