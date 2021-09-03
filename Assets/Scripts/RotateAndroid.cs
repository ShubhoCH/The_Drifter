using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAndroid : MonoBehaviour
{
    float speedOfRotation = 100f;
    bool left,right;
    float initials;
    void Start()
    {
        left = right = false;
        initials = speedOfRotation;
    }
    void FixedUpdate()
    {
        if(Input.touchCount > 0){
            if(left){
                if(right == true)
                    right = false;
                this.transform.RotateAround(new Vector3(0,0,0), new Vector3(0,0,1f),-speedOfRotation * Time.deltaTime);
            }

            if(right){
                if(left == true)
                    left = false;
                this.transform.RotateAround(new Vector3(0,0,0), new Vector3(0,0,1f),speedOfRotation * Time.deltaTime);
            }
        }
        else{
            if(left == true || right == true)
                left = right = false;
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
    public void LeftButton(){
        if(left == false)
            left = true;
    }
    public void RightButton(){
        if(right == false)
            right = true;
    }
}
