using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    public void PlaySound(){
        FindObjectOfType<AudioManager>().Play("Click");
    }
}
