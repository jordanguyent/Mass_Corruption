using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Attack : MonoBehaviour
{

    private float timeTracker;
    private float timeTracker2;
    private float timeCooldown;
    private int attackCounter = 1;
    private bool isAttacking = false;
    public int magicNumber = 3;

    private float offSet = 1;

    public GameObject sideAttack1;
    public GameObject sideAttack2;
    public GameObject sideAttack3;
    public GameObject upAttack;
    public GameObject downAttack;
    public GameObject magicAttack;
    GameObject tempAttack;
    GameObject tempAttack2;
    GameObject tempAttack3;
    GameObject tempAttack4;
    Rigidbody2D rb2D;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerAttack();
        timeTracker += Time.deltaTime;
        timeTracker2 += Time.deltaTime;
        timeCooldown += Time.deltaTime;

        if (tempAttack != null)
        {
            tempAttack.transform.position = new Vector2(transform.position.x + offSet, transform.position.y);           
        }
        if(tempAttack2 != null)
        {
            tempAttack2.transform.position = new Vector2(transform.position.x, transform.position.y + 1);
        }
    }

    void PlayerAttack()
    {

        //SIDE ATTACK
        if (Input.GetKeyDown(KeyCode.X) && !Input.GetKey(KeyCode.UpArrow) && !isAttacking)
        {

            //Reset Attack Variables
            isAttacking = true;
            timeTracker = 0;
            timeCooldown = 0;

            //Attack 1
            if (attackCounter == 1)
            {
                tempAttack = Instantiate(sideAttack1);
            }
            //Attack 2
            else if (attackCounter == 2)
            {
                tempAttack = Instantiate(sideAttack2);
            }
            //Attack 2
            else if (attackCounter == 3)
            {
                tempAttack = Instantiate(sideAttack3);
            }

            //Position of Attack
            if(tempAttack != null)
            {
                if (GetComponent<SpriteRenderer>().flipX == true)
                {
                    offSet = -1;
                    tempAttack.GetComponent<SpriteRenderer>().flipX = true;
                }
                else if (GetComponent<SpriteRenderer>().flipX == false)
                {
                    offSet = 1;
                    tempAttack.GetComponent<SpriteRenderer>().flipX = false;
                }
            }
            attackCounter++;
        }

        //UP ATTACK
        if (Input.GetKeyDown(KeyCode.X) && Input.GetKey(KeyCode.UpArrow) && !isAttacking)
        {
            timeTracker2 = 0;
            timeCooldown = 0;
            attackCounter = 1;
            tempAttack2 = Instantiate(upAttack);
            isAttacking = true;
        }

        //DOWN ATTACK
        if (Player_Movement.jumpCounter == 2)
        {
            Player_Movement.jumpCounter++;
            tempAttack3 = Instantiate(downAttack);
            tempAttack3.transform.position = new Vector2(transform.position.x, transform.position.y - 0.7f);
            tempAttack3.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -7);
        }
        //MAGIC ATTACK
        if (Input.GetKeyDown(KeyCode.V) && magicNumber > 0)
        {
            magicNumber--;
            tempAttack4 = Instantiate(magicAttack);
            if (GetComponent<SpriteRenderer>().flipX == false)
            {
                tempAttack4.GetComponent<SpriteRenderer>().flipX = false;
                tempAttack4.transform.position = new Vector2(transform.position.x + 1f, transform.position.y);
                tempAttack4.GetComponent<Rigidbody2D>().velocity = new Vector2(12, 0);
            }
            else if (GetComponent<SpriteRenderer>().flipX == true)
            {
                tempAttack4.GetComponent<SpriteRenderer>().flipX = true;
                tempAttack4.transform.position = new Vector2(transform.position.x - 1f, transform.position.y);
                tempAttack4.GetComponent<Rigidbody2D>().velocity = new Vector2(-12, 0);
            }
        }

        //RESET ATTACKS
        if (tempAttack != null)
        {
            if (timeTracker >= 0.1667 && attackCounter != 4)
            {
                Destroy(tempAttack);
                isAttacking = false;
            }
            else if (timeTracker >= 0.2915)
            {
                Destroy(tempAttack);
                isAttacking = false;
            }
        }
        if (tempAttack2 != null)
        {
            if (timeTracker2 >= 0.2085)
            {
                Destroy(tempAttack2);
            }
        }
        
        if (timeCooldown >= 0.4)
        {
            attackCounter = 1;
            isAttacking = false;
        }
    }
}
