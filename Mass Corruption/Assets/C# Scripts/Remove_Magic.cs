using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Remove_Magic : MonoBehaviour
{
    private float aniTime = 5;

    private void Update()
    {
        aniTime += Time.deltaTime;
        if(aniTime > .166 && aniTime < 3)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        aniTime = 0;
        GetComponent<Animator>().SetBool("isHit", true);
    }
}
