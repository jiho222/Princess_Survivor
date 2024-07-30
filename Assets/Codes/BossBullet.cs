using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D collision) // 총알이 부딪히면 총알 삭제
    {
        if (!collision.CompareTag("Player"))
            return;

        Player player = collision.GetComponent<Player>();
        if(player != null)
        {
            player.TakeDamage();
        }    
        rigid.velocity = Vector2.zero;
        Destroy(gameObject);
    }

    void OnTriggerExit2D(Collider2D collision) // 밖으로 나가면 총알 삭제
    {
        if (!collision.CompareTag("Area"))
            return;

        Destroy(gameObject);
    }
}
