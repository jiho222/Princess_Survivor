using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public int per;

    Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, int per, Vector3 dir)
    {
        this.damage = damage; // 왼쪽: bullet 안의 damage, 오른쪽: 매개변수로 받은 damage
        this.per = per;

        if (per >= 0) {
            rigid.velocity = dir * 15f;
        }
    }

    void OnTriggerEnter2D(Collider2D collision) // 맞을때마다 per을 감소시킨다
    {
        if (!collision.CompareTag("Enemy") || per == -100) // Enemy에 닿은게 아니거나 per이 -100(근접무기)가 아닐경우에
            return;

        per --;

        if (per < 0) {
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }

    void OnTriggerExit2D(Collider2D collision) // 밖으로 나가면 총알 삭제
    {
        if (!collision.CompareTag("Area") || per == -100)
            return;

        gameObject.SetActive(false);
    }
}
