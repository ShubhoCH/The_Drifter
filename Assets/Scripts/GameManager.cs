using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject rewardedVideoCanvas;
    public GameObject endGameCanvas;
    public float delay = 3f;
    bool rewardedOnce;
    public void Start(){
        rewardedOnce = false;
    }
    public void Die()
    {
        Invoke("Restart",delay);
    }
    void Restart()
    {
        if(rewardedOnce != true)
            rewardedVideoCanvas.SetActive(true);
        else    
            endGameCanvas.SetActive(true);
    }
    public void Check(){
        rewardedOnce = true;
    }
}
