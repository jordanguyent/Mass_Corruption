using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Down_Attack_onHit : MonoBehaviour
{
    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer >= 0.333)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GetComponent<CapsuleCollider2D>().enabled = false;
        GetComponent<Rigidbody2D>().gravityScale = 0;
        GetComponent<Animator>().SetBool("onHit", true);
    }
}
