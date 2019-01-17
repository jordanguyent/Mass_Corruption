using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wisp_Health : MonoBehaviour
{
    public int health = 3;
    private int r = 255;
    private float flashTimer = 5;
    private float deathTimer = 0;

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
            r = 0;
        }
        else
        {
            r = 255;
        }
        GetComponent<SpriteRenderer>().color = new Color(r, 255, 255, 175);
    }

    void Death()
    {
        if (health <= 0)
        {
            deathTimer += Time.deltaTime;
            GetComponent<Rigidbody2D>().gravityScale = 1;
            GetComponent<Wisp_Movement>().enabled = false;
            gameObject.tag = "Untagged";
            GetComponent<Animator>().SetBool("isDead", true);
            if (deathTimer >= 2 && deathTimer < 2.33)
            {
                gameObject.tag = "Enemy";
                GetComponent<CircleCollider2D>().radius = .7f;
                GetComponent<CircleCollider2D>().
                transform.localScale = new Vector2(2, 2);
                GetComponent<Animator>().SetBool("isExplode", true);
            }
            else if(deathTimer >= 2.33)
            {
                Destroy(gameObject);
            }
            
        }
    }
}
