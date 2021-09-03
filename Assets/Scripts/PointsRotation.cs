using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsRotation : MonoBehaviour
{
    void Update()
    {
        this.transform.Rotate(0,45f * Time.deltaTime,0);
    }
}
