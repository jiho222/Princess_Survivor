using System.Collections;
using UnityEngine;

public class Boss : Enemy
{
    public float shootingRange;
    public float shootCooldown;
    public float lastShootTime;
    public GameObject bossBullet;

    void Update()
    {
        if (!GameManager.instance.isLive || !isLive)
            return;

        

        // 플레이어가 사정거리 내에 있는지 확인
        float distanceToPlayer = Vector2.Distance(target.transform.position, transform.position);
        if (distanceToPlayer <= shootingRange)
        {
            lastShootTime += Time.deltaTime;
            if (lastShootTime >= shootCooldown)
            {
                Shoot();
                lastShootTime = 0f;
            }
        }
    }

    void Shoot()
    {
        Vector2 dir = (target.transform.position - transform.position).normalized;
        GameObject bbullet = Instantiate(bossBullet, transform);
        bbullet.transform.position = transform.position;
        bbullet.transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bbullet.GetComponent<Rigidbody2D>().velocity = dir * 10f;
    }

    protected override void Die()
    {
        base.Die();
        StartCoroutine(DestroyAndActivateBox());
    }

    IEnumerator DestroyAndActivateBox()
    {
        GameObject box = transform.Find("Box").gameObject; // 박스 아이템 찾기
        box.transform.SetParent(null); // 아이템의 부모를 null로 설정하여 분리
        box.SetActive(true); // 아이템 활성화
        yield return new WaitForSeconds(0.1f); // 한 프레임 대기하여 치킨 활성화가 반영되도록 함
        Destroy(gameObject); // 현재 오브젝트 삭제
    }
}
