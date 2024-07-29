using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData; // 레벨마다 데이터가 필요하니까 배열
    public float levelTime;

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
    }

    void Spawn()
    {
        GameObject enemy = GameManager.instance.pool.Get(0);
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position; // 0번째는 부모 오브젝트이므로 1부터 시작
        enemy.GetComponent<Enemy>().Init(spawnData[level]);

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