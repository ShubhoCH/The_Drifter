using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistanceRemaining : MonoBehaviour
{
    float remDistance = 2650f;
    public Transform playerTransform;
    public Text remDistanceText;
    void Update()
    {
        remDistanceText.text = (remDistance - playerTransform.position.z).ToString("0");
        if(playerTransform.transform.position.z >= remDistance)
            remDistance += 2100f;
    }

}
