using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;


public class ScoreManagement : MonoBehaviour
{
    public GameObject character, finalScore;
    public RectTransform plus1000;
    public int score = 0;
    int scoreInUI = 0;
    void Start(){
        InvokeRepeating("addScore", 5, 5);    
    }

    void addScore(){
        if(character.GetComponent<CharacterMovement>().playerAlive){
            score += 1000;
            StartCoroutine(plus1000Animation());
        }
    }

    IEnumerator plus1000Animation(){
        plus1000.DOAnchorPos(new Vector2(200, 0), 0f, true);
        plus1000.DOAnchorPos(new Vector2(380, 110), 1f, true);
        yield return new WaitForSeconds(1f);
        StartCoroutine(scoreIncrementAnimation());
        plus1000.DOAnchorPos(new Vector2(5000, 5000), 0f, true);
    
    }

    IEnumerator scoreIncrementAnimation(){
        finalScore.GetComponent<TMP_Text>().text = score.ToString(); 
        while(scoreInUI < score){
            scoreInUI+=5;
            yield return new WaitForSeconds(0.002f);
            gameObject.GetComponent<TMP_Text>().text = "Score " + scoreInUI.ToString(); 
        }
    }

}
