using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;

    float timer;
    Player player;

    void Awake()
    {
        player = GameManager.instance.player;
    }

    // void Start()
    // {
    //     Init();
    // }
    
    void Update()
    {
        // if (!GameManager.instance.isLive)
        //     return;
            
        switch (id) {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;
            default:
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
        this.damage = damage; // * Character.Damage;
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
        damage = data.baseDamage; // * Character.Damage;
        count = data.baseCount; // + Character.Count;

        // 프리펩 아이디는 풀링 매니저의 변수에서 찾아서 초기화
        // 스크립트블 오브젝트의 독립성을 위해서 인덱스가 아닌 프리펩으로 설정
        for (int index=0; index < GameManager.instance.pool.prefabs.Length; index++) {
            if (data.projectile == GameManager.instance.pool.prefabs[index]) {
                prefabId = index;
                break;
            }
        }

        switch (id) {
            case 0:
                speed = 150; // * Character.WeaponSpeed;
                Batch();
                break;
            default:
                speed = 0.5f; // * Character.WeaponRate;
                break;
        }

    //     // Hand Set
    //     Hand hand = player.hands[(int)data.itemType];
    //     hand.spriter.sprite = data.hand;
    //     hand.gameObject.SetActive(true);

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
            // BroadcastMessage 는 특정 함수 호출을 모든 자식에게 방송하는 함수
    }

    void Batch()
    {
        for (int index = 0; index < count; index++) {
            Transform bullet;
            
            if (index < transform.childCount) { // 기존 오브젝트를 재활용하고 없는것을 풀링
                bullet = transform.GetChild(index);
            }
            else {
                bullet = GameManager.instance.pool.Get(prefabId).transform;
            }
             
            bullet.parent = transform; // pool이 부모인 상태에서 player의 자식으로 들어감

            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * index / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);
            bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero); // -100 is Infinity Per. // bullet 컴포넌트 접근하여 속성 초기화 함수 호출
        }
    }

    void Fire()
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
}
