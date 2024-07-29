using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpPickUp : MonoBehaviour
{
    private bool movingToPlayer;
    public float moveSpeed = -3f;

    public float timeBetweenChecks = .2f;
    public float checkCounter;

    void Update()
    {
        if(movingToPlayer == true)
        {
            transform.position = Vector2.MoveTowards(transform.position, GameManager.instance.player.transform.position, moveSpeed * Time.deltaTime);
        } else
        {
            checkCounter -= Time.deltaTime;
            if(checkCounter <= 0)
            {
                checkCounter = timeBetweenChecks;
                if(Vector2.Distance(transform.position, GameManager.instance.player.transform.position) < GameManager.instance.player.pickupRange)
                {
                    movingToPlayer = true;
                    moveSpeed += GameManager.instance.player.speed;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.GetExp();
            Destroy(gameObject);
        }
    }
}
