using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class PlayAgain : MonoBehaviour
{

    void Update(){
        if(Input.GetKeyDown("r")){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            transform.DORestart();

        }
    }
}
