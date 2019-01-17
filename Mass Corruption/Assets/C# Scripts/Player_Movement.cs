using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{

    public int speed = 3;
    public int jumpPower = 900;
    public float maxJumpMultiplier = 7f;
    public float lowJumpMultiplier = 5f;
    private bool isGrounded;
    private bool isWalking;
    public static int jumpCounter = 0;
    private float backupTimer;

    Rigidbody2D rb2D;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
        PlayerRaycast();

        //in case you cant jump
        backupTimer += Time.deltaTime;
        if (backupTimer > 2)
        {
            isGrounded = true;
            jumpCounter = 0;
        }
        if(jumpCounter > 2)
        {
            isGrounded = false;
        }
    }

    void PlayerMove()
    {

        //HORIZONTAL MOVEMENT
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb2D.velocity = new Vector2(speed, rb2D.velocity.y);
            GetComponent<SpriteRenderer>().flipX = false;
            isWalking = true;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb2D.velocity = new Vector2(-speed, rb2D.velocity.y);
            GetComponent<SpriteRenderer>().flipX = true;
            isWalking = true;
        }
        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            rb2D.velocity = new Vector2(0, rb2D.velocity.y);
            isWalking = false;
        }
        //VERTICAL MOVEMENT
        if (Input.GetKeyDown(KeyCode.C) && (isGrounded || jumpCounter < 2))
        {
            jumpCounter++;
            backupTimer = 0;
            rb2D.AddForce(Vector2.up * jumpPower);
            rb2D.velocity = new Vector2(rb2D.velocity.x, 0);
        }

        if (rb2D.velocity.y < 0)  //faster fall
        {
            rb2D.velocity += Vector2.up * Physics2D.gravity.y * (maxJumpMultiplier - 1) * Time.deltaTime;  //Time.deltaTime is how many seconds in a frame
        }
        else if (rb2D.velocity.y > 0 && !Input.GetKey(KeyCode.C))
        {
            rb2D.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        //ANIMATION
        if (isWalking)
        {
            GetComponent<Animator>().SetBool("isWalking", true);
        }
        else
        {
            GetComponent<Animator>().SetBool("isWalking", false);
        }
        if (!isGrounded)
        {
            GetComponent<Animator>().SetBool("isJumping", true);
            if (rb2D.velocity.y < 0)
            {
                GetComponent<Animator>().SetBool("jumpTransition", true);
            }
            else
            {
                GetComponent<Animator>().SetBool("jumpTransition", false);
            }
        }
        else if (isGrounded)
        {
            GetComponent<Animator>().SetBool("isJumping", false);
        }
    }

    //COLLISIONS
    void PlayerRaycast()
    {
        //DOWNWARDS RAY
        RaycastHit2D rayDownR = Physics2D.Raycast(new Vector2(transform.position.x + 0.345f, transform.position.y), Vector2.down);
        RaycastHit2D rayDownL = Physics2D.Raycast(new Vector2(transform.position.x - 0.345f, transform.position.y), Vector2.down);
        Debug.DrawRay(new Vector2(transform.position.x + 0.345f, transform.position.y), Vector2.down * .48f);
        Debug.DrawRay(new Vector2(transform.position.x - 0.345f, transform.position.y), Vector2.down * .48f);

        //RIGHT RAYCAST
        if (rayDownR.collider != null && rayDownR.distance < 0.48f && rayDownR.collider.tag == "Jumpable")
        {
            backupTimer = 0;
            isGrounded = true;
            jumpCounter = 0;
        }
        //LEFT RAYCAST
        else if (rayDownL.collider != null && rayDownL.distance < 0.48f && rayDownL.collider.tag == "Jumpable")
        {
            backupTimer = 0;
            isGrounded = true;
            jumpCounter = 0;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GetComponent<Animator>().SetBool("isWalking", false);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(jumpCounter == 0)
        {
            isGrounded = false;
            jumpCounter = 1;
        }
    }
}
