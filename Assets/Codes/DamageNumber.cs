using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageNumber : MonoBehaviour
{
    public TMP_Text damageText;

    public float lifetime;
    private float lifeCounter;

    void Start()
    {
        lifeCounter = lifetime;
    }

    void Update()
    {
        if(lifeCounter > 0)
        {
            lifeCounter -= Time.deltaTime;

            if(lifeCounter <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void Setup(int damageDisplay)
    {
        lifeCounter = lifetime;

        damageText.text = damageDisplay.ToString(); // damageText.text 는 TextMeshPro속의 text
    }
}
