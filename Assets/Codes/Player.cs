using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed; 
    public Scanner scanner;
    public Hand[] hands;
    public RuntimeAnimatorController[] animCon;

    public float pickupRange = 1.5f;

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        hands = GetComponentsInChildren<Hand>(true);
    }

    void OnEnable() // 캐릭터를 실행
    {
        speed *= Character.Speed;
        anim.runtimeAnimatorController = animCon[GameManager.instance.playerId];
    }

    void FixedUpdate() // 이동
    {
        if (!GameManager.instance.isLive)
            return;
                    
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }

    void OnMove(InputValue value) // Input 받음
    {
        inputVec = value.Get<Vector2>();
    }

    void LateUpdate() // 이동, 좌우반전
    {
        if (!GameManager.instance.isLive)
            return;
        
         anim.SetFloat("Speed", inputVec.magnitude); // magnitude는 벡터의 순수한 크기 값

        if (inputVec.x != 0) {
            spriter.flipX = inputVec.x < 0;
        }
    }

    void OnCollisionStay2D (Collision2D collision) // 피격
    {
        if (!GameManager.instance.isLive)
            return;

        GameManager.instance.health -= Time.deltaTime * 10; // 프레임마다 적절한 피격 데미지 계산
        anim.SetTrigger("isHit");

        if ( GameManager.instance.health < 0) {
            for (int index = 2; index < transform.childCount; index++) {
                transform.GetChild(index).gameObject.SetActive(false);
            }

            anim.SetTrigger("isDead");
            GameManager.instance.GameOver();
        }
    }
}
