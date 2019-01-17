using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knightmage_Movement : MonoBehaviour
{

    //Position
    private float distance;
    private float playerPositionX;
    private float playerPositionY;
    private float knightPositionX;
    private float knightPositionY;

    //Movement
    public float fieldOfVision = 5;
    private float xVel = 2.5f;
    public float patrolDistX1 = 0;
    public float patrolDistX2 = 50;
    private float posTrackX = 0;
    private float randVel;

    //Attack
    private float attackTimer;
    private float cooldown;
    private bool isAttacking;
    private bool weaponOut;
    public GameObject swordAttack;
    GameObject attack;

    GameObject Player;

    // Start is called before the first frame update
    void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        posTrackX = Random.Range(patrolDistX1, patrolDistX2);
        randVel = Random.Range(1, 2);
        if (randVel == 1)
        {
            xVel = 2f;
        }
        else if (randVel == 2)
        {
            xVel = -2f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        attackTimer += Time.deltaTime;
        cooldown += Time.deltaTime;
        FindDistance();
        Movement();
        EnemyRaycast();
        Attack();
    }

    void FindDistance()
    {
        playerPositionX = Player.transform.position.x;
        playerPositionY = Player.transform.position.y;
        knightPositionX = transform.position.x;
        knightPositionY = transform.position.y;
        distance = Mathf.Sqrt(Mathf.Pow((playerPositionX - knightPositionX), 2) + Mathf.Pow((playerPositionY - knightPositionY), 2));

        if (distance < fieldOfVision)
        {
            Debug.DrawRay(new Vector2(knightPositionX, knightPositionY), new Vector2(playerPositionX - knightPositionX, playerPositionY - knightPositionY), Color.red);
            Debug.DrawRay(new Vector2(knightPositionX, knightPositionY), new Vector2(playerPositionX - knightPositionX, 0), Color.green);
            Debug.DrawRay(new Vector2(knightPositionX, knightPositionY), new Vector2(0, playerPositionY - knightPositionY), Color.green);
        }
    }

    void Movement()
    {
        if (distance < fieldOfVision && !isAttacking)
        {
            //X MOVEMENT
            if (transform.position.x < Player.transform.position.x - 1.75f)
            {
                xVel = 2.5f;  
                GetComponent<Animator>().SetBool("isWalking", true);
            }
            else if (transform.position.x > Player.transform.position.x + 1.75f)
            {
                xVel = -2.5f;
                GetComponent<Animator>().SetBool("isWalking", true);
            }
            else //initiate attack
            {
                if (cooldown >= 1.5f)
                {
                    isAttacking = true;
                    attackTimer = 0;
                }
                xVel = 0;
                GetComponent<Animator>().SetBool("isWalking", false);
            }  

            //velocity
            GetComponent<Rigidbody2D>().velocity = new Vector2(xVel, 0);


            //flip sprite
            if (transform.position.x > Player.transform.position.x)
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (transform.position.x < Player.transform.position.x)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
        }


        //Patrol Movement
        else
        {
            if (posTrackX <= patrolDistX1)
            {
                xVel = 2f;
                GetComponent<SpriteRenderer>().flipX = true;
            }
            else if (posTrackX >= patrolDistX2)
            {
                xVel = -2f;
                GetComponent<SpriteRenderer>().flipX = false;
            }
            posTrackX += xVel / 10;
            GetComponent<Rigidbody2D>().velocity = new Vector2(xVel, 0);
            GetComponent<Animator>().SetBool("isWalking", true);
        }
    }

    void Attack()
    {
        if (isAttacking)
        {
            if (attackTimer > .8336)
            {
                GetComponent<Animator>().SetBool("isAttacking", false);
                isAttacking = false;
                cooldown = 0;
                weaponOut = false;
            }
            else if (attackTimer > .5) //attacking
            {
                if (!weaponOut)
                {
                    attack = Instantiate(swordAttack);
                    weaponOut = true;
                    if (GetComponent<SpriteRenderer>().flipX == false)
                    {
                        attack.GetComponent<SpriteRenderer>().flipX = false;
                        attack.transform.position = new Vector2(transform.position.x - .5f, transform.position.y);
                        attack.GetComponent<Rigidbody2D>().angularVelocity = 300;
                    }
                    else if (GetComponent<SpriteRenderer>().flipX == true)
                    {
                        attack.GetComponent<SpriteRenderer>().flipX = true;
                        attack.transform.position = new Vector2(transform.position.x + .5f, transform.position.y);
                        attack.GetComponent<Rigidbody2D>().angularVelocity = -300;
                    }
                }
                
                GetComponent<Animator>().SetBool("isAttacking", true);
            }
            else if(attackTimer > 0)
            {
                GetComponent<Animator>().SetBool("isWalking", false);
            }
        }
    }

    void EnemyRaycast()
    {
        RaycastHit2D rayRight = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.right);
        RaycastHit2D rayLeft = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.left);
        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y), Vector2.right * .5f);
        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y), Vector2.left * .5f);

        //Right raycast
        if (rayRight.collider != null && rayRight.distance < .5f && xVel != 0 && rayRight.collider.tag != "Player" && rayRight.collider.tag != "Enemy")
        {
            xVel = -2.5f;
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (rayLeft.collider != null && rayLeft.distance < .5f && xVel != 0 && rayLeft.collider.tag != "Player" && rayLeft.collider.tag != "Enemy")
        {
            xVel = 2.5f;
            GetComponent<SpriteRenderer>().flipX = true;
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
