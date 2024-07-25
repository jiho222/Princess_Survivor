using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    Collider2D coll;

    void Awake()
    {
        coll = GetComponent<Collider2D>();
    }

    void OnTriggerExit2D(Collider2D collision) // Ground 가 Player 안의 Area와 충돌하면 재배치
    {
        if (!collision.CompareTag("Area"))
            return;

        Vector3 playerPos = GameManager.instance.player.transform.position; // 플레이어 위치
        Vector3 myPos = transform.position; // Ground 위치


        switch (transform.tag) {  // Ground 뿐만 아니라 Enemy도 재배치 해줘야 함
            case "Ground": 
                float diffX = playerPos.x - myPos.x;
                float diffY = playerPos.y - myPos.y;
                float dirX = diffX < 0 ? -1 : 1;
                float dirY = diffY < 0 ? -1 : 1;
                diffX = Mathf.Abs(diffX);
                diffY = Mathf.Abs(diffY);

                if (diffX > diffY) {
                    transform.Translate(Vector3.right * dirX * 80); // x축으로 두칸 이동
                }
                else if (diffX < diffY) {
                    transform.Translate(Vector3.up * dirY * 80); // y축으로 두칸 이동
                }
                break;
            case "Enemy":
                if (coll.enabled) {
                    Vector3 dist = playerPos;
                    Vector3 ran = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
                    transform.Translate(ran + dist * 2); // 1배는 플레이어랑 붙음, 2배는 플레이어 반대쪽에서 재등장
                }
                break;
        }
    }
}