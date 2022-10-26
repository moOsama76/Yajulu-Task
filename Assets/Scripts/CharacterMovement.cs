using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CharacterMovement : MonoBehaviour
{
    public GameObject camera, fakeLand, fakeBody, healthBar, rightArm, leftArm, skateBoard, gameOver, score, distance, plus1000;
    float camDistanceY, camDistanceZ, fwdSpeed = 0.5f, sideSpeed = 0.25f, animationTime = 0.8f, gravityScale = 15, tiltScale = 10f, tiltDuration = .2f;
    int gravityDirection = 1, timeInSpace = 0, floatingLimit = 5, overFloatingPunishment = 7, pumbingCost = 3;
    public int currentHealth = 100;
    bool validFlipAnimation = true, playerOverFloating = false, pumbingProtection = false;
    public bool playerAlive = true;
    Rigidbody characterRigidbody;
    public Animator characterAnimator;

    bool checkGround(){
        return Physics.Raycast(transform.position, new Vector3(0, -1*gravityDirection, 0), 2);
    }

    void reverseGravity(){
            Physics.gravity = new Vector3(0, gravityDirection*gravityScale, 0);
    }
    
    void removeEnergy(){
            characterRigidbody.velocity = Vector3.zero;
            characterRigidbody.angularVelocity = Vector3.zero;
    }

    void flipAnimation(float rotZ, char pressed){
        if(pressed == 'e')
            transform.DORotate((new Vector3(0, 0, rotZ+90)) , animationTime/2, RotateMode.Fast);
        else
            transform.DORotate((new Vector3(0, 0, rotZ-90)) , animationTime/2, RotateMode.Fast);
        transform.DORotate((new Vector3(0, 0, rotZ)) , animationTime/2, RotateMode.Fast);
    }

    void floatingAnimation(){
        rightArm.transform.DORotate((new Vector3(-35, -95, 0)) , animationTime, RotateMode.Fast);
        leftArm.transform.DORotate((new Vector3(-30, -20, 50)) , animationTime, RotateMode.Fast);
    }

    void standingAnimation(){
        rightArm.transform.DORotate((new Vector3(36, -26, -30)) , animationTime, RotateMode.Fast);
        leftArm.transform.DORotate((new Vector3(45, 12, -2)) , animationTime, RotateMode.Fast);
    }

    void skateBoardLoseAnimation(){
        skateBoard.transform.parent = null;
        skateBoard.transform.DOMove(new Vector3(skateBoard.transform.position.x, skateBoard.transform.position.y, skateBoard.transform.position.z+10), 1f, false);
    }

    public void applyDamage(int removedHealth){
            RectTransform barTransform = healthBar.GetComponent<RectTransform>();
            currentHealth -= removedHealth;
            barTransform.DOSizeDelta(new Vector2(currentHealth, 50), 0.1f, true);
            barTransform.DOAnchorPosX(barTransform.anchoredPosition.x - removedHealth/2.0f, 0f, false);
    }

    void lose(){
            playerAlive = false;
            gameOver.SetActive(true);
            score.SetActive(false);
            distance.SetActive(false);
            plus1000.SetActive(false);
            
    }

    void Start(){
        camDistanceZ = camera.transform.position.z - transform.position.z;
        camDistanceY = camera.transform.position.y - transform.position.y;
        characterRigidbody = GetComponent<Rigidbody>();
        InvokeRepeating("overFloatingCheck", 1f, 1f);

    }

    void OnCollisionEnter(Collision collision){

        if (collision.gameObject.tag == "enemyClone"){
            if(!pumbingProtection){
                applyDamage(12);
                Destroy(collision.gameObject);
                StartCoroutine(hitAnimation());
            }
        }
    }

    void Update(){
        
        fakeLand.transform.position = new Vector3(fakeLand.transform.position.x , fakeLand.transform.position.y, transform.position.z+150);
        camera.transform.eulerAngles = transform.eulerAngles;
        camera.transform.position = new Vector3(transform.position.x*0.75f, transform.position.y + camDistanceY*gravityDirection, transform.position.z + camDistanceZ);
        if(playerAlive){
            if (Input.GetKeyDown("e") && validFlipAnimation){
                    removeEnergy();
                    reverseGravity();
                    validFlipAnimation = false;
                    StartCoroutine(releaseLock());
                    if(gravityDirection == 1){
                        flipAnimation(-180, 'e');
                    } else {
                        flipAnimation(0, 'e');
                    }
                    gravityDirection *= -1;
                } else if (Input.GetKeyDown("q") && validFlipAnimation){
                    removeEnergy(); 
                    reverseGravity();
                    validFlipAnimation = false;
                    StartCoroutine(releaseLock());
                    if(gravityDirection == 1){
                        flipAnimation(180, 'q');
                    } else {
                        flipAnimation(360, 'q');
                    }
                    gravityDirection *= -1;
                } 
            characterAnimator.SetInteger("health", currentHealth);
            if (checkGround()){
                characterAnimator.SetBool("onGround", true);
            } else {
                characterAnimator.SetBool("onGround", false);
            }
            if(!pumbingProtection && currentHealth > pumbingCost && checkGround() && (Input.GetKeyDown("up") || Input.GetKeyDown("w"))){
                StartCoroutine(pumbing());
            }
            if(currentHealth <= 0 && playerAlive){
                if(checkGround()){
                    skateBoardLoseAnimation();
                } else {
                    GetComponent<CapsuleCollider>().enabled = false;
                    skateBoard.SetActive(false);
                }
                lose();
            }

        }
        
    }


    void FixedUpdate(){
        if(playerAlive){
            transform.Translate(0, 0, fwdSpeed);
            if (Input.GetKey("right")){
                if(gravityDirection == 1)
                    fakeBody.transform.DORotate((new Vector3(0, 20, -tiltScale)) , tiltDuration, RotateMode.Fast);
                else
                    fakeBody.transform.DORotate((new Vector3(0, -20, 180-tiltScale)) , tiltDuration, RotateMode.Fast);
                transform.Translate(sideSpeed, 0, 0);
            } else if (Input.GetKey("left")){
                if(gravityDirection == 1)
                    fakeBody.transform.DORotate((new Vector3(0, -20, tiltScale)) , tiltDuration, RotateMode.Fast);
                else
                    fakeBody.transform.DORotate((new Vector3(0, 20, 180+tiltScale)) , tiltDuration, RotateMode.Fast);
                transform.Translate(-sideSpeed, 0, 0);
            } else {
                if(gravityDirection == 1)
                    fakeBody.transform.DORotate((new Vector3(0, 0, 0)) , tiltDuration, RotateMode.Fast);
                else
                    fakeBody.transform.DORotate((new Vector3(0, 0, 180)) , tiltDuration, RotateMode.Fast);
            }
        }
    }

    IEnumerator releaseLock(){
        yield return new WaitForSeconds(animationTime);
        validFlipAnimation = true;
    }

    IEnumerator pumbing(){
            applyDamage(pumbingCost);
            transform.DOMove(new Vector3(transform.position.x, transform.position.y, transform.position.z+20), 0.2f, false);
            pumbingProtection = true;
            characterAnimator.SetBool("pumbing", true);
            yield return new WaitForSeconds(0.2f);
            characterAnimator.SetBool("pumbing", false);
            pumbingProtection = false;

    }

    IEnumerator hitAnimation(){
        fwdSpeed = 0;
        sideSpeed = 0;
        yield return new WaitForSeconds(0.1f);
        fwdSpeed = 0.5f;
        sideSpeed = 0.25f;
    }


    void overFloatingCheck(){
        if (checkGround()){
            characterAnimator.SetBool("onGround", true);
            timeInSpace = 0;
            playerOverFloating = false;
        } else {
            characterAnimator.SetBool("onGround", false);
            timeInSpace++;
            if(timeInSpace == floatingLimit){
                playerOverFloating = true;
            }
        }

        if(playerOverFloating){
            applyDamage(overFloatingPunishment);
        }
    }
}
