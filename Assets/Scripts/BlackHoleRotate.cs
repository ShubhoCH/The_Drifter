using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleRotate : MonoBehaviour
{
    void FixedUpdate()
    {
        this.transform.RotateAround(new Vector3(0,0,0),  new Vector3(0,0,1), 30f * Time.deltaTime);
    }
}
