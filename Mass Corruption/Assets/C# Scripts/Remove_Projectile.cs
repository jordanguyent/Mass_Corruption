using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Remove_Projectile : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Enemy" && collision.gameObject.tag != "PlayerAttack" && collision.gameObject.tag != "Player" || transform.position.y >= 1000)
        {
            Destroy(gameObject);
        }
    }
}
