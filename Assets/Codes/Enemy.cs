using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxhealth;
    public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target;

    bool isLive;

    Rigidbody2D rigid;
    Collider2D coll;
    Animator anim;
    SpriteRenderer spriter;
    WaitForFixedUpdate wait;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();
    }    

    void FixedUpdate() // 플레이어 추적, (+Hit상태일시 멈춤)
    {
        if (!GameManager.instance.isLive)
            return;

        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit")) // Hit 애니메이션이 실행중이면 멈춤 // 화살표이름이아닌 상태의이름
            return;      
        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;

        // Speed 값을 계산하여 애니메이터에 설정
        float speedValue = nextVec.magnitude / Time.fixedDeltaTime;
        anim.SetFloat("Speed", speedValue);
    }

    void LateUpdate() // 좌우반전
    {
        if (!GameManager.instance.isLive)
            return;
        spriter.flipX = target.position.x < rigid.position.x;
    }

    void OnEnable() // 오브젝트풀링에서 프리펩을 가져오면 타겟은 설정되지 않기 때문에 여기서 따로 타겟을 가져온다
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        coll.enabled = true;
        rigid.simulated = true;
        spriter.sortingOrder = 3;
        anim.SetBool("isDead", false);
        health = maxhealth;
    }

    public void Init(SpawnData data) // Spawner에서 지정해준 SpawnData를 받아야함
    {
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxhealth = data.health;
        health = data.health;
    }

    void OnTriggerEnter2D(Collider2D collision) // 맞으면 피깍임, 넉백, 사망
    {
        if (!collision.CompareTag("Bullet") || !isLive)
            return;

        health -= collision.GetComponent<Bullet>().damage;
        StartCoroutine(KnockBack());

        if (health > 0) {
            anim.SetTrigger("isHit");
            // AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);
        }
        else {
            isLive = false;
            coll.enabled = false;
            rigid.simulated = false;
            spriter.sortingOrder = 2;
            anim.SetBool("isDead", true);
            GameManager.instance.kill++;
            StartCoroutine(ExpDrop());

            // if (GameManager.instance.isLive)
            //     AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);
        }

        DamageNumberController.instance.SpawnDamage(collision.GetComponent<Bullet>().damage, transform.position);
    }

    IEnumerator KnockBack()
    {
        yield return wait; // 다음 하나의 물리 프레임 딜레이
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse); // Impulse: 즉발적인 힘을 가함
    }

    IEnumerator ExpDrop()
    {
        GameObject exppoint = transform.Find("ExpPoint").gameObject;
        exppoint.transform.SetParent(null);
        exppoint.SetActive(true);
        yield return new WaitForSeconds(2); // 4초 기다림
        gameObject.SetActive(false);
    }

    void Dead() // 애니메이션이 실행시켜준다 // 오류로 실행 안됨
    {
        // gameObject.SetActive(false);
    }
}
