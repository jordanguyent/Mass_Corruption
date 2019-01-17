using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wisp_Movement : MonoBehaviour
{
    //Position
    private float distance;
    private float playerPositionX;
    private float playerPositionY;
    private float wispPositionX;
    private float wispPositionY;

    //Movement
    public float fieldOfVision = 5;
    private float yVel;
    private float xVel;
    public float patrolDistX1 = 0;
    public float patrolDistX2 = 30;
    private float posTrackX = 0;
    private float randVel;


    GameObject Player;

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
        Movement();
    }

    void FindDistance()
    {
        playerPositionX = Player.transform.position.x;
        playerPositionY = Player.transform.position.y;
        wispPositionX = transform.position.x;
        wispPositionY = transform.position.y;
        distance = Mathf.Sqrt(Mathf.Pow((playerPositionX - wispPositionX), 2) + Mathf.Pow((playerPositionY - wispPositionY), 2));

        //Developer Purposes
        if (distance < fieldOfVision)
        {
            Debug.DrawRay(new Vector2(wispPositionX, wispPositionY), new Vector2(playerPositionX - wispPositionX, playerPositionY - wispPositionY), Color.red);
            Debug.DrawRay(new Vector2(wispPositionX, wispPositionY), new Vector2(playerPositionX - wispPositionX, 0), Color.green);
            Debug.DrawRay(new Vector2(wispPositionX, wispPositionY), new Vector2(0, playerPositionY - wispPositionY), Color.green);
        }
    }

    void Movement()
    {

        if(distance < fieldOfVision)
        {
            //SPRITE FLIPPING
            if (playerPositionX < wispPositionX)
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (playerPositionX > wispPositionY)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }

            //X MOVEMENT
            if (transform.position.x < Player.transform.position.x)
            {
                if (xVel < 3f)
                {
                    xVel += 0.1f;
                }
            }
            else if (transform.position.x > Player.transform.position.x)
            {
                if (xVel > -3f)
                {
                    xVel -= 0.1f;
                }
                
            }
            if(distance < fieldOfVision - 2)
            {
                if (transform.position.y > Player.transform.position.y)
                {
                    if (yVel > -3f)
                    {
                        yVel -= 0.1f;
                    }
                }
            }
            if (transform.position.y < Player.transform.position.y)
            {
                if (yVel < 3f)
                {
                    yVel += 0.15f;
                }
            }

            GetComponent<Rigidbody2D>().velocity = new Vector2(xVel, yVel);
        }
        //Patrol movement
        else
        {
            if (posTrackX <= patrolDistX1)
            {
                xVel = 1.5f;
                GetComponent<SpriteRenderer>().flipX = true;
            }
            else if (posTrackX >= patrolDistX2)
            {
                xVel = -1.5f;
                GetComponent<SpriteRenderer>().flipX = false;
            }
            posTrackX += xVel / 10;
            GetComponent<Rigidbody2D>().velocity = new Vector2(xVel, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Jumpable")
        {
            yVel = 3;
        }
        if (collision.gameObject.tag == "Enemy")
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }
}
