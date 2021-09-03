using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuRotate : MonoBehaviour
{
    float rotateWith = 6f;
    void Start()
    {
        Invoke("Change",10f);
    }
    void Update()
    {
        this.transform.Rotate(new Vector3(0,0,rotateWith) * Time.deltaTime, Space.World);
    }
    void Change()
    {
        rotateWith *= -1f;
        Invoke("Change",10f);
    }
}
