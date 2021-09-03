using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    float speedOfRotation = 100f;
    float initials;
    void Start()
    {
        initials = speedOfRotation;
    }
    void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.A)){
            this.transform.RotateAround(new Vector3(0,0,0), new Vector3(0,0,1f),speedOfRotation * Time.deltaTime);
        }

        if(Input.GetKey(KeyCode.D)){
            this.transform.RotateAround(new Vector3(0,0,0), new Vector3(0,0,1f),-speedOfRotation * Time.deltaTime);
        }
    }
    public void Difficulty()
    {
        speedOfRotation += 5;
    }
    public void AdjustForNewSpace(){
        speedOfRotation = initials + 10;
        initials += 10;
    }
}