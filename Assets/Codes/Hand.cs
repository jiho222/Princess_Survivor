using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Hand : MonoBehaviour
{
    public bool isLeft;
    public SpriteRenderer spriter;

    SpriteRenderer player;

    Vector3 leftPos = new Vector3(0.7f, 0, 0);
    Vector3 leftPosReverse = new Vector3(-0.7f, 0, 0);
    Vector3 rightPos = new Vector3(0.6f, 0.2f, 0);
    Vector3 rightPosReverse = new Vector3(-0.6f, 0.2f, 0);
    // Quaternion leftRot = Quaternion.Euler(0, 0, -35);
    // Quaternion leftRotReverse = Quaternion.Euler(0, 0, -135);

    void Awake()
    {
        player = GetComponentsInParent<SpriteRenderer>()[1];
    }

    void LateUpdate()
    {
        bool isReverse = player.flipX;

        if (isLeft) { // 근접무기
            // transform.localRotation = isReverse ? leftRotReverse : leftRot;
            // spriter.flipY = isReverse;
            transform.localPosition = isReverse ? leftPosReverse : leftPos;
            spriter.flipX = isReverse;
            spriter.sortingOrder = isReverse ? 4 : 6;
        }
        else { // 원거리무기
            transform.localPosition = isReverse ? rightPosReverse : rightPos;
            spriter.flipX = isReverse;
            spriter.sortingOrder = isReverse ? 6 : 4;
        }
    }
}
