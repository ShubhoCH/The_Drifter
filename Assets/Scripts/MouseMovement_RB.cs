using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement_RB : MonoBehaviour
{
    Animator animator;
    public GameObject Player;
    public Transform cheeseParent;
    public GameObject playerCollider;
    public GameObject particle;
    public GameObject cam;
    int count = 0;
    public Rigidbody rb;
    public float forwardForce = 20f;
    float initialForwardForce;
    bool onCheese;
    bool inAir = false;
    bool isGrounded = false;
    public TraumaInducer tr;
    public float rotateOnCheese = 1f;
    public float translateOnCheese = 1/3;
    float levelUpDistance = 100f;
    public GameObject character;
    bool inFlight = false;
    public ObstacleManager om;
    public Score ui;
    float limiter = 40f;
    public TouchControl tcl;
    public TouchControlRight tcr;
    public GameObject jumpB;
    public GameObject jumpBImage;
    public int camMaxCount = 12;
    public float camMaxTranslate = 1f;
    public float playerDeadPosition;
    bool dead = false;
    void Start()
    {
        jumpB.SetActive(false);
        initialForwardForce = forwardForce;
        onCheese = false;
        inAir = false;
        animator = GetComponentInChildren<Animator>();
        FindObjectOfType<AudioManager>().Play("Background");
        particle.SetActive(false);
        playerDeadPosition = 0f;
    }
    void FixedUpdate()
    {
        //rb.AddForce(new Vector3(0,-1,0) * 250f);
        if(Player.transform.position.y < -9){
            if(isGrounded == false)
            {
                isGrounded = true;
                inAir = false;
                animator.SetBool("colBlackhole",false);
                animator.SetBool("endFlight",false);
                animator.SetBool("isGrounded",true);
                animator.SetBool("jump",false);
            }
        }
        Player.transform.Translate(0,0,forwardForce * Time.deltaTime);
        //rb.AddForce(0, 0, forwardForce * Time.deltaTime, ForceMode.VelocityChange);
        if(Player.transform.position.y > 6.5f)
        {
            StartCoroutine(tr.Shake());
            Die();
        }
        if(Player.transform.position.z > levelUpDistance)
        {
            if(forwardForce <= limiter){
                tcl.Difficulty();
                tcr.Difficulty();
                Difficulty();
            }
            levelUpDistance += 100f;
        }
        if(count != 0)
        {
            animator.SetBool("colBlackhole",false);
            jumpB.SetActive(true);
            jumpBImage.SetActive(true);
            particle.SetActive(true);
        }
        else{
            jumpB.SetActive(false);
            jumpBImage.SetActive(false);
            particle.SetActive(false);
        }
        if(Player.transform.position.y < 0f && inFlight == true)
            Player.transform.Translate(0,12*Time.deltaTime,0);
        if(Player.transform.position.y > 0f && inFlight == true)
            Player.transform.Translate(0,-12*Time.deltaTime,0);
    }
    public void Jump(){
        animator.SetBool("jump",true);
        animator.SetBool("onCheese",false);
        animator.SetBool("isGrounded",false);
        onCheese = false;
        do{
            particle.transform.localPosition += new Vector3(0,1,0);
            if(count <= camMaxCount){
                cam.transform.localPosition += new Vector3(0,-1/9f,translateOnCheese);
                cam.transform.localEulerAngles += new Vector3(-rotateOnCheese,0,0);
            }
            else{
                cam.transform.localPosition += new Vector3(0,1f,translateOnCheese + camMaxTranslate);
                cam.transform.localEulerAngles += new Vector3(-rotateOnCheese + 5,0,0);
            }
            cheeseParent.transform.GetChild(--count).parent = null;
        }while(count != 0);
        Player.transform.position += new Vector3(0,2f,0);
        inAir = true;
    }
    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Cheese")
        {
            particle.transform.localPosition -= new Vector3(0,1,0);
            animator.SetBool("onCheese",true);
            col.transform.parent = cheeseParent.transform;
            col.isTrigger = false;  
            count++;
            if(count == 1)
            {
                animator.SetBool("isGrounded",false);
                animator.SetBool("jump",false);
            }
            onCheese = true;
            if(isGrounded == true)
            {
                isGrounded = false;
            }
            Player.transform.position += new Vector3(0, 1, 0);
            col.transform.localPosition = new Vector3(0,-1*count,0);
            if(count <= camMaxCount){
                cam.transform.localPosition += new Vector3(0,1/9f,-translateOnCheese);
                cam.transform.localEulerAngles += new Vector3(rotateOnCheese,0,0);
            }
            else{
                cam.transform.localPosition += new Vector3(0,-1f,-translateOnCheese - camMaxTranslate);
                cam.transform.localEulerAngles += new Vector3(rotateOnCheese-5,0,0);
            }
        }
        if(col.gameObject.tag == "Orb")
        {
            FindObjectOfType<AudioManager>().Play("Fly");
            StartCoroutine(tr.Shake());
            if(onCheese == true)
            {
                onCheese = false;
                do{
                    particle.transform.localPosition += new Vector3(0,1,0);
                    if(count <=camMaxCount){
                        cam.transform.localPosition += new Vector3(0,-1/9f,translateOnCheese);
                        cam.transform.localEulerAngles += new Vector3(-rotateOnCheese,0,0); 
                    } 
                    else{
                        cam.transform.localPosition += new Vector3(0,1f,translateOnCheese + camMaxTranslate);
                        cam.transform.localEulerAngles += new Vector3(-rotateOnCheese + 5,0,0);
                    }
                    cheeseParent.transform.GetChild(--count).parent = null;
                    ui.ScoreUpdater();
                }while(count != 0);
            }
            animator.SetBool("onCheese",false);
            om.CollectPoints();
            om.ClearObstacleWhenEnteringNewSpace();
            Fly();
        }
        if(col.gameObject.tag == "Portal")
        {
            if (inFlight)
            {
                EndFlight();
                om.StopCollectingPoints();
                om.ClearObstacleWhenEnteringNewSpace();
                om.ClearPointsWhenEnteringNewSpace();
                forwardForce = initialForwardForce + 5f;
            }
            else{
                tcl.AdjustForNewSpace();
                tcr.AdjustForNewSpace();
            }
            initialForwardForce += 5;
        }
        if(col.gameObject.tag == "Blackhole")
        {
            animator.SetBool("colBlackhole",true);
            animator.SetBool("onCheese",false);
            if(count > 0)
            {
                onCheese = false;
                do{
                    particle.transform.localPosition += new Vector3(0,1,0);
                    if(count <= camMaxCount){
                        cam.transform.localPosition += new Vector3(0,-1/9f,translateOnCheese);
                        cam.transform.localEulerAngles += new Vector3(-rotateOnCheese,0,0);
                    }
                    else{
                        cam.transform.localPosition += new Vector3(0,1f,translateOnCheese + camMaxTranslate);
                        cam.transform.localEulerAngles += new Vector3(-rotateOnCheese + 5,0,0);
                    }
                    cheeseParent.transform.GetChild(--count).parent = null;
                }while(count != 0);
                animator.SetBool("colBlackhole",true);
                animator.SetBool("onCheese",false);
                inAir = true;
            }
            else{
                character.SetActive(false);
                Die();
            }
            
        }
        /*if(col.gameObject.tag == "Obstacle")
        {
            col.isTrigger = false;
            if(count == 0)
            {
                animator.SetBool("dead",true);
                FindObjectOfType<GameManager>().Die();
                //FindObjectOfType<GameManager>().EndGame();
            } 
            if(count != 0 && count > 1)
            {
                cheeseParent.transform.GetChild(--count).parent = null;
                cheeseParent.transform.GetChild(--count).parent = null;
                if(count == 0)
                {
                    onCheese = false;
                    animator.SetBool("onCheese",false);
                    animator.SetBool("jump",false);
                    animator.SetBool("isGrounded",true);
                }
            }
        }
        if(col.gameObject.tag == "Break")
        {
            col.isTrigger = false;
            //col.transform.Rotate(new Vector3(1f,0,0) * 90f * Time.deltaTime);
            if(onCheese == false && inAir)
            {
                //breakScript.breakTheObject();
            }
        }*/
    }
    void OnCollisionEnter(Collision hit)
    {
        /*if(hit.collider.tag == "Ground")
        {
            if(isGrounded == false)
            {
                isGrounded = true;
                inAir = false;
                animator.SetBool("colBlackhole",false);
                animator.SetBool("endFlight",false);
            }
            animator.SetBool("isGrounded",true);
            animator.SetBool("jump",false);
        }*/
        if(hit.collider.tag == "Collidant")
        {
            if(count == 0)
            {
                Die();
            } 
            else{
                particle.transform.localPosition += new Vector3(0,1,0);
                cheeseParent.transform.GetChild(--count).parent = null;
                ui.ScoreUpdater();
                if(count <= camMaxCount){
                    cam.transform.localPosition += new Vector3(0,-1/9f,translateOnCheese);
                    cam.transform.localEulerAngles += new Vector3(-rotateOnCheese,0,0);
                }
                else{
                    cam.transform.localPosition += new Vector3(0,1f,translateOnCheese + camMaxTranslate);
                    cam.transform.localEulerAngles += new Vector3(-rotateOnCheese + 5,0,0);
                }
                if(count == 0)
                {
                    onCheese = false;
                    animator.SetBool("onCheese",false);
                    animator.SetBool("jump",false);
                    animator.SetBool("isGrounded",true);
                }
            }
        }
        if(hit.collider.tag == "Stop")
        {
            Die();
        }
    }
    void Difficulty()
    {
        forwardForce += 0.5f;
    }
    public void Die()
    {
        if(!dead){
            //FindObjectOfType<AudioManager>().Stop("Background");
            FindObjectOfType<AudioManager>().Play("Die");
            if(playerDeadPosition == 0){
                playerDeadPosition = Player.transform.position.z;
            }
            forwardForce = 0f;
            if(count > 0)
            {
                onCheese = false;
                do{
                    particle.transform.localPosition += new Vector3(0,1,0);
                    if(count <= camMaxCount){
                        cam.transform.localPosition += new Vector3(0,-1/9f,translateOnCheese);
                        cam.transform.localEulerAngles += new Vector3(-rotateOnCheese,0,0);
                    }
                    else{
                        cam.transform.localPosition += new Vector3(0,1f,translateOnCheese + camMaxTranslate);
                        cam.transform.localEulerAngles += new Vector3(-rotateOnCheese + 5,0,0);
                    }
                    cheeseParent.transform.GetChild(--count).parent = null;
                }while(count != 0);
                animator.SetBool("onCheese",false);
                inAir = true;
                rb.AddForce(0,0,-10000f);
            }
            else{
                rb.AddForce(0,0,-20000f);
            }
            animator.SetBool("dead",true);
            FindObjectOfType<GameManager>().Die();
            dead = true;
        }
    }
    void Fly(){
        rb.useGravity = false;
        forwardForce = 300f;
        animator.SetBool("fly",true);
        inFlight = true;
        isGrounded = false;
        animator.SetBool("isGrounded",false);
    }
    void EndFlight(){
        rb.useGravity = true;
        inFlight = false;
        tcl.AdjustForNewSpace();
        tcr.AdjustForNewSpace();
        animator.SetBool("endFlight",true);
        animator.SetBool("fly",false);
    }
    public void Revive(){
        dead = false;
        character.SetActive(true);
        om.ClearObstacleWhenReviving();
        om.ClearPointsWhenReviving();
        animator.SetBool("dead",false);
        forwardForce = initialForwardForce;
    }
    public float R(){
        return playerDeadPosition;
    }
}

//Player.transform.Translate(0,0,forwardForce * Time.deltaTime);

/*if(Input.GetKey(KeyCode.D)){
    this.transform.RotateAround(new Vector3(0,0,0), new Vector3(0,0,1f),-90f * Time.deltaTime);
    //rb.AddForce(sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
    //Player.transform.Translate(sidewaysForce * Time.deltaTime, 0, 0);
}

if(Input.GetKey(KeyCode.A)){
    this.transform.RotateAround(new Vector3(0,0,0), new Vector3(0,0,1f),90f * Time.deltaTime);
    //rb.AddForce(-sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
    //Player.transform.Translate(-sidewaysForce * Time.deltaTime, 0, 0);
}*/

/*if(Input.GetKeyDown(KeyCode.S) && isGrounded && !onCheese && !inAir){
    rb.AddForce(0, 0, slideForce, ForceMode.VelocityChange);
    animator.SetBool("isSliding",true);
    isSliding = true;
    playerCollider.transform.eulerAngles = new Vector3(-90f,0,0);
    //Player.transform.Translate(-sidewaysForce * Time.deltaTime, 0, 0);
}*/