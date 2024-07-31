using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("# Game Control")]
    public bool isLive;
    public float gameTime;
    public float maxGameTime;
    public float bossSpawnTime;
    [Header("# Player Info")]
    public int playerId;
    public float health;
    public float maxHealth = 100;
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
    [Header("# GameObject")]
    public PoolManager pool;
    public Player player;
    public LevelUp uiLevelUp;
    public Result uiResult;
    public Transform uiJoy;
    public GameObject enemyCleaner;
    [Header("# Boss")]
    public GameObject boss1;
    public GameObject boss2;
    private bool boss1Spawned = false;
    private bool boss2Spawned = false;

    void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60;
    }

    public void GameStart(int id)
    {
        playerId = id;
        health = maxHealth;

        player.gameObject.SetActive(true);
        // uiLevelUp.Select(playerId % 2); // 캐릭터 늘어나면 기본무기 지급이 안될 수 있기 때문에 2로 나눈 나머지

        if (playerId == 0)
        {
            uiLevelUp.Select(1);
        }
        else if (playerId == 1)
        {
            uiLevelUp.Select(5);
        }        

        Resume(); // 게임 시작 시 Time.timeScale = 1

        AudioManager.instance.PlayBgm(true);

    }

    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        isLive = false;

        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);
        uiResult.Lose();
        Stop();

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);
        AudioManager.instance.PlayBgm(false);
    }
    
    public void GameVictory()
    {
        StartCoroutine(GameVictoryRoutine());
    }

    IEnumerator GameVictoryRoutine()
    {
        isLive = false;
        enemyCleaner.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);
        uiResult.Win();
        Stop();

        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Win);
    }


    public void GameRetry()
    {
        SceneManager.LoadScene(0); // Build Setting의 0번째 씬을 로드
    }

    public void GameQuit()
    {
        Application.Quit();
    }

    void Update() 
    {
        if (!isLive)
            return;

        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime) {
            gameTime = maxGameTime;
            GameVictory();
        }

        BossSpawn();
    }

    public void GetExp()
    {
        if (!isLive)
            return; // 게임이 끝나면 경험치 획득 불가

        exp ++;

        if (exp == nextExp[Mathf.Min(level, nextExp.Length -1)]) { // 레벨과 관련된 nextExp
            level++;
            exp = 0;
            uiLevelUp.Show();
            AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);
        }
    }

    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0;
        uiJoy.localScale = Vector3.zero;
    }

    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;
        uiJoy.localScale = Vector3.one;
    }

    void BossSpawn()
    {
        // 2분(120초)일 때 boss1을 스폰, 단 한 번만
        if (GameManager.instance.gameTime >= 120f && !boss1Spawned)
        {
            boss1.SetActive(true);
            boss1.transform.position = GameManager.instance.player.transform.position + Vector3.up * 8f;
            boss1Spawned = true; // Boss1이 이미 스폰되었음을 기록
        }

        // 4분(240초)일 때 boss2를 스폰, 단 한 번만
        if (GameManager.instance.gameTime >= 240f && !boss2Spawned)
        {
            boss2.SetActive(true);
            boss2.transform.position = GameManager.instance.player.transform.position + Vector3.up * 8f;
            boss2Spawned = true; // Boss2가 이미 스폰되었음을 기록
        }
    }
}
