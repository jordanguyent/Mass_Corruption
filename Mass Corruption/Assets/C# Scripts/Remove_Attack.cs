using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Remove_Attack : MonoBehaviour
{
    public float deleteTime = .3336f;
    private float removeTimer = 0;

    // Update is called once per frame
    void Update()
    {
        removeTimer += Time.deltaTime;
        if (removeTimer > deleteTime)
        {
            Destroy(gameObject);
        }
    }
}
