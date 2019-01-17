using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gheist_Movement : MonoBehaviour
{
    //Position
    private float distance;
    private float playerPositionX;
    private float playerPositionY;
    private float gheistPositionX;
    private float gheistPositionY;

    //Movement
    public float fieldOfVision = 5;
    private float yVel;
    private float xVel = 1.5f;
    public float patrolDistX1 = 0;
    public float patrolDistX2 = 30;
    private float posTrackX = 0;
    private float randVel;

    //Attacks
    public float attackCooldown = 3;
    public float attackTime;
    private float attackVel = 5;



    public GameObject Fireball;
    GameObject Player;
    GameObject FireAttack;

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        posTrackX = Random.Range(patrolDistX1, patrolDistX2);
        randVel = Random.Range(1, 2);
        if (randVel == 1)
        {
            xVel = 1.5f;
        }
        else if (randVel == 2)
        {
            xVel = -1.5f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        FindDistance();
        Attack();
        Movement();
        
    }

    void FindDistance()
    {
        playerPositionX = Player.transform.position.x;
        playerPositionY = Player.transform.position.y;
        gheistPositionX = transform.position.x;
        gheistPositionY = transform.position.y;
        distance = Mathf.Sqrt(Mathf.Pow((playerPositionX - gheistPositionX), 2) + Mathf.Pow((playerPositionY - gheistPositionY), 2));
    }

    void Attack()
    {
        attackTime += Time.deltaTime;

        if (distance < fieldOfVision)
        {
            float xComp = playerPositionX - gheistPositionX;
            float yComp = playerPositionY - gheistPositionY;
            //Show Axis
            Debug.DrawRay(new Vector2(gheistPositionX, gheistPositionY), new Vector2(xComp, yComp), Color.red);
            Debug.DrawRay(new Vector2(gheistPositionX, gheistPositionY), new Vector2(xComp, 0), Color.green);
            Debug.DrawRay(new Vector2(gheistPositionX, gheistPositionY), new Vector2(0, yComp), Color.green);

            //Creates fireball
            if (attackTime > attackCooldown)
            {
                FireAttack = Instantiate(Fireball);
                FireAttack.transform.position = new Vector2(transform.position.x, transform.position.y);
                FireAttack.GetComponent<CircleCollider2D>().isTrigger = true;
                FireAttack.GetComponent<Rigidbody2D>().velocity = new Vector2(xComp, yComp).normalized * attackVel;
                attackTime = 0;
            }
        }
    }

    void Movement()
    {
        if (distance < fieldOfVision)
        {
            //SPRITE FLIPPING
            if (playerPositionX < gheistPositionX)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
            else if (playerPositionX > gheistPositionX)
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }

            

            //X MOVEMENT
            if (transform.position.x < Player.transform.position.x - 3)
            {
                yVel = Random.Range(-.5f, .5f);
                xVel = 1.5f;
            }
            else if (transform.position.x > Player.transform.position.x + 3)
            {
                yVel = Random.Range(-.5f, .5f);
                xVel = -1.5f;
            }


            //Changes the Y position if too high or too low
            if (transform.position.y > Player.transform.position.y + 2)
            {
                yVel = -1.5f;
            }
            else if (transform.position.y < Player.transform.position.y + .5f)
            {
                yVel = 1.5f;
            }

            GetComponent<Rigidbody2D>().velocity = new Vector2(xVel, yVel);
        }

        //Patrol movement
        else
        {
            if (posTrackX <= patrolDistX1)
            {
                xVel = 1.5f;
                GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (posTrackX >= patrolDistX2)
            {
                xVel = -1.5f;
                GetComponent<SpriteRenderer>().flipX = true;
            }  
            posTrackX += xVel/10;
            GetComponent<Rigidbody2D>().velocity = new Vector2(xVel, 0);
        }
    }

    //ignores collision with other enemies
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }
}
