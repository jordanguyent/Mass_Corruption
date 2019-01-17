using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Health : MonoBehaviour
{
    public int health = 5;
    private int b = 255;
    private int g = 255;
    private int a = 255;
    private float flashTimer = 5;

    // Update is called once per frame
    void Update()
    {
        flashTimer += Time.deltaTime;
        HitAni();
        Death();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlayerAttack")
        {
            health--;
            flashTimer = 0;
        }
        else if (collision.gameObject.tag == "PlayerAttack2")
        {
            health -= 2;
            flashTimer = 0;
        }
    }

    void HitAni()
    {
        if (flashTimer < .125f)
        {
            a = 150;
            b = 0;
            g = 0; 
        }
        else
        {
            b = 255;
            g = 255;
            a = 255;
        }
        GetComponent<SpriteRenderer>().color = new Color(255, g, b, a);
    }

    void Death()
    {
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
