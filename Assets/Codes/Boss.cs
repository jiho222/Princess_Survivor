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
}
