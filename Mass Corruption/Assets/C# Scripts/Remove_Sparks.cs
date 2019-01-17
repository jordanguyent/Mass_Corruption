using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Remove_Sparks : MonoBehaviour
{
    private float aniTimer = 0;

    // Update is called once per frame
    void Update()
    {
        aniTimer += Time.deltaTime;
        if(aniTimer > .166)
        {
            Destroy(gameObject);
        }
    }
}
