using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadCollisionCheck : MonoBehaviour
{
    public MouseMovement_RB mm;
    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Head")
        {
            mm.Die();
        }
    }
}
