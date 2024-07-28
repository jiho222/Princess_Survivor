using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;

    public SpriteRenderer spriter;

    float timer;
    Player player;

    void Awake()
    {
        player = GameManager.instance.player;
    }

    void Update()
    {
        if (!GameManager.instance.isLive)
            return;
            
        switch (id) {
            case 0: // 왕관
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;
            case 5: // 방망이
                timer += Time.deltaTime;

                if (timer > speed) {
                    timer = 0f;
                    Swing();
                }
                break;
            default: // 마법지팡이
                timer += Time.deltaTime;

                if (timer > speed) {
                    timer = 0f;
                    Fire();
                }
                break;
        }
    }

    public void LevelUp(float damage, int count)
    {
        this.damage = damage * Character.Damage;
        this.count += count;

        if (id ==0)
            Batch();

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    public void Init(ItemData data)
    {
        // Basic Set
        name = "Weapon " + data.itemId;
        transform.parent = player.transform; // 플레이어 자식으로 설정
        transform.localPosition = Vector3.zero;

        // Property Set
        id = data.itemId;
        damage = data.baseDamage * Character.Damage;
        count = data.baseCount + Character.Count;

        // 프리펩 아이디는 풀링 매니저의 변수에서 찾아서 초기화
        // 스크립트블 오브젝트의 독립성을 위해서 인덱스가 아닌 프리펩으로 설정
        for (int index=0; index < GameManager.instance.pool.prefabs.Length; index++) {
            // projectile 프리펩이 풀링 매니저의 프리펩과 같으면 인덱스를 저장
            if (data.projectile == GameManager.instance.pool.prefabs[index]) {
                prefabId = index;
                break;
            }
        }

        switch (id) {
            case 0:
                speed = 150 * Character.WeaponSpeed;
                Batch();
                break;
            case 5:
                speed = 1.0f * Character.WeaponRate;
                break;
                
            default:
                speed = 0.5f * Character.WeaponRate;
                break;
        }

        // Hand Set
        Hand hand = player.hands[(int)data.itemType]; // Player.cs의 Hand 배열에 접근하여 hand에 할당, data.itemType은 ItemData.cs의 열거형에서 가져옴
        hand.spriter.sprite = data.hand;
        hand.gameObject.SetActive(true);

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
            // BroadcastMessage 는 특정 함수 호출을 모든 자식에게 방송하는 함수
    }

    void Batch() // 빙빙도는무기 설정
    {
        for (int index = 0; index < count; index++) {
            Transform bullet;
            
            if (index < transform.childCount) { // 기존 오브젝트를 재활용하고 없는것을 풀링
                bullet = transform.GetChild(index);
            }
            else {
                bullet = GameManager.instance.pool.Get(prefabId).transform;
            }
             
            bullet.parent = transform; // pool 자식인 상태에서 player의 자식으로 됨

            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * index / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);
            bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero); // -100 is Infinity Per. // bullet 컴포넌트 접근하여 속성 초기화 함수 호출
        }
    }

    void Fire() // 원거리무기 설정
    {
        if (!player.scanner.nearestTarget)
            return;
        // 총알방향계산
        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;
        // 위치, 회전 결정, bullet 스크립트에게 전달
        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir); // FromToRotation: 지정된 축을 중심으로 목표를 향해 회전하는 함수
        bullet.GetComponent<Bullet>().Init(damage, count, dir);

        // AudioManager.instance.PlaySfx(AudioManager.Sfx.Range);
    }

    void Swing()
    {
        Transform bat = GameManager.instance.pool.Get(prefabId).transform;
        bat.parent = transform; // 플레이어의 자식으로 설정
        bat.localPosition = Vector3.up * 2.5f; // 플레이어 위로 이동
        bat.localRotation = Quaternion.Euler(0, 0, 45f); // 초기 각도 45도로 설정

        bat.gameObject.SetActive(true); // 방망이 활성화

        bat.GetComponent<Bullet>().Init(damage, -100, Vector3.zero); // -100 is Infinity Per.

        // Player 의 FlipX 여부에 따라 방향을 결정
        if (player.spriter.flipX)
        {
            // 플레이어가 좌측을 바라볼 때
            StartCoroutine(SwingRoutine(bat, 45f, 135f, 180f)); // 반시계 방향 휘두르기
        }
        else
        {
            // 플레이어가 우측을 바라볼 때
            StartCoroutine(SwingRoutine(bat, -45f, -135f, 180f)); // 시계 방향 휘두르기
        }
    }

    IEnumerator SwingRoutine(Transform bat, float startAngle, float endAngle, float swingSpeed)
    {
        float currentAngle = startAngle;
        bool isSwingingClockwise = startAngle > endAngle;

        while ((isSwingingClockwise && currentAngle > endAngle) || (!isSwingingClockwise && currentAngle < endAngle))
    {
        currentAngle += (isSwingingClockwise ? -1 : 1) * swingSpeed * Time.deltaTime;

        // while (currentAngle > endAngle)
        // {
        //     currentAngle -= swingSpeed * Time.deltaTime;

            // 방망이의 위치와 회전을 함께 업데이트
            bat.position = transform.position + Quaternion.Euler(0, 0, currentAngle) * Vector3.up * 2.5f;
            bat.rotation = transform.rotation * Quaternion.Euler(0, 0, currentAngle);

            yield return null; // 한 프레임 대기
        }

        bat.localPosition = Vector3.up * 2.5f; // 초기 위치로 되돌림
        bat.localRotation = Quaternion.Euler(0, 0, 45f); // 초기 각도로 되돌림
        bat.gameObject.SetActive(false); // 방망이를 비활성화하여 오브젝트 풀로 반환
    }

}