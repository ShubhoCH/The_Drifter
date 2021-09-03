using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendulam : MonoBehaviour
{
    float rotateWith = 60f;
    bool enough = false;
    void Start()
    {
        Invoke("Enough",10f);
    }
    void FixedUpdate()
    {
        this.transform.Rotate(new Vector3(0,0,rotateWith) * Time.deltaTime, Space.World);
    }
    void Change()
    {
        if(enough == true)
        {
            rotateWith *= -1f;
            Invoke("Change",3f);
        }
    }
    void Enough()
    {
        enough = true;
        Invoke("Change",3f);
    }

}