using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Breakable_Wall : MonoBehaviour
{

    private int health = 20;
    private float aniTimer;
    GameObject player;
    public GameObject sparks;
    private bool sparking = false;

    private void Start()
    {
        if(GameObject.FindGameObjectWithTag("Player") != null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        
    }

    private void Update()
    {
        aniTimer += Time.deltaTime;
        Anim();
        if (health <= 0)
        {
            Death();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlayerAttack")
        {
            health--;
            aniTimer = 0;

            if(!sparking)
            {
                Instantiate(sparks);
                sparks.transform.position = collision.gameObject.transform.position;
                sparking = true;
            }

            if(collision.gameObject.GetComponent<SpriteRenderer>().flipX == true)
            {
                sparks.GetComponent<SpriteRenderer>().flipX = true;
            }
            else if (collision.gameObject.GetComponent<SpriteRenderer>().flipX == false)
            {
                sparks.GetComponent<SpriteRenderer>().flipX = false;
            }
        }
    }

    void Anim()
    {
        if (aniTimer > .05)
        {
            sparking = false;
        }
        transform.position = new Vector2(0, 0);
    }

    void Death()
    {
        Destroy(gameObject);
    }
}
