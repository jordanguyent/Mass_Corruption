using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Health : MonoBehaviour
{

    //TODO Fix player collision after being hit. He glitches into the ground if too close to ground when hit
    private int health = 3;
    private float hitTimer;
    private bool isHit = false;
    private float backupTimer;
    private float defaultGravityScale;

    private void Start()
    {
        defaultGravityScale = GetComponent<Rigidbody2D>().gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        hitTimer += Time.deltaTime;
        backupTimer += Time.deltaTime;
        if (isHit == true) {
            HitAni();
            BackupCode();
        }
        
    }

    //TRIGGER EVENT
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyAttack" || collision.gameObject.tag == "EnemyAttack2" && !isHit)
        {
            if (collision.gameObject.tag == "EnemyAttack")
            {
                Destroy(collision.gameObject);
            }
            isHit = true;
            hitTimer = 0;
            backupTimer = 0;
            health--;
            Debug.Log(health);

            if (transform.position.x < collision.gameObject.transform.position.x)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(-10, 0);
                GetComponent<Rigidbody2D>().rotation = 30;
            }
            else if (transform.position.x >= collision.gameObject.transform.position.x)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(10, 0);
                GetComponent<Rigidbody2D>().rotation = -30;
            }
        }
    }

    //DAMAGE ON HIT ANIMATION
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && !isHit)
        {
            isHit = true;
            hitTimer = 0;
            backupTimer = 0;
            health--;
            Debug.Log(health);

            if (transform.position.x < collision.gameObject.transform.position.x)
            {              
                GetComponent<Rigidbody2D>().velocity = new Vector2(-10, 0);
                GetComponent<Rigidbody2D>().rotation = 30;
            }
            else if (transform.position.x >= collision.gameObject.transform.position.x)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(10, 0);
                GetComponent<Rigidbody2D>().rotation = -30;          
            }
        }
    }

    //RESETS BACK TO NORM
    void HitAni()
    {
        if (hitTimer >= 0 && hitTimer <= .125)
        {
            //Animation sake
            GetComponent<SpriteRenderer>().color = new Color(0, 255, 255, 25);
            GetComponent<Rigidbody2D>().gravityScale = 0;
            GetComponent<Player_Attack>().enabled = false;
            GetComponent<Player_Movement>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
        }
        else if (hitTimer > .125 && hitTimer <= .2)
        {
            GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
        }
        if (hitTimer > .2 && hitTimer <= .21)
        {
            GetComponent<Player_Attack>().enabled = true;
            GetComponent<Player_Movement>().enabled = true;
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            GetComponent<BoxCollider2D>().enabled = true;
            GetComponent<Rigidbody2D>().gravityScale = defaultGravityScale;
            GetComponent<Rigidbody2D>().rotation = 0;
            
        }
        else if (hitTimer > 1)
        {
            isHit = false;
        }
    }

    //IF RESET CODE EVER FAILS
    void BackupCode()
    {
        if (backupTimer > .21 && backupTimer < .22)
        {
            GetComponent<Player_Attack>().enabled = true;
            GetComponent<Player_Movement>().enabled = true;
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            GetComponent<BoxCollider2D>().enabled = true;
            GetComponent<Rigidbody2D>().gravityScale = defaultGravityScale;
            GetComponent<Rigidbody2D>().rotation = 0;
        }
    }
}
