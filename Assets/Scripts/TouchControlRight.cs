using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchControlRight : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    bool ispressed = false;
    public GameObject obmag;
    float speedOfRotation = 100f;
    float initials;
    void Start()
    {
        initials = speedOfRotation;
    }
    void Update()
    {
        if(ispressed){
            obmag.transform.RotateAround(new Vector3(0,0,0), new Vector3(0,0,1f),-speedOfRotation * Time.deltaTime);
        }
    }
    public void OnPointerDown(PointerEventData eventData){
        ispressed = true;
    }
    public void OnPointerUp(PointerEventData eventData){
        ispressed = false;
    }
    public void Difficulty()
    {
        speedOfRotation += 1.5f;
    }
    public void AdjustForNewSpace(){
        speedOfRotation = initials + 10;
        initials += 10;
    }
}
