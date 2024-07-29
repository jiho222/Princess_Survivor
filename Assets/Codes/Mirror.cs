using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    public float damage;
    public int per;

    Rigidbody2D rigid;
    Player player;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        player = GameManager.instance.player;
    }

    public void Init(float damage, int per, Vector3 dir)
    {
        this.damage = damage; // 왼쪽: bullet 안의 damage, 오른쪽: 매개변수로 받은 damage
        this.per = per;

        if (per >= 0) {
            rigid.velocity = dir * 7f;
        }
    }

    void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        Vector2 velocity = rigid.velocity;
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);

        bool reflectX = viewportPosition.x <= 0 || viewportPosition.x >= 1;
        bool reflectY = viewportPosition.y <= 0 || viewportPosition.y >= 1;

        if (reflectX || reflectY)
        {
            // 반사 처리
            if (reflectX)
                velocity.x = -velocity.x;
            if (reflectY)
                velocity.y = -velocity.y;
            
            // 속도 업데이트
            rigid.velocity = velocity;

            // 위치 보정
            Vector3 correctedViewportPosition = new Vector3(
                Mathf.Clamp(viewportPosition.x, 0.01f, 0.99f),
                Mathf.Clamp(viewportPosition.y, 0.01f, 0.99f),
                viewportPosition.z
            );
            Vector3 correctedPosition = Camera.main.ViewportToWorldPoint(correctedViewportPosition);
            correctedPosition.z = transform.position.z; // z축 위치는 유지
            transform.position = correctedPosition;
        }
    }
}

    