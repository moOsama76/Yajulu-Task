using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Distance : MonoBehaviour
{
    GameObject character;
    public int distance;  
    void Start(){
        character = GameObject.FindGameObjectWithTag("Player");
    }

    void Update(){
        
        distance = (int)character.transform.position.z/10;
        gameObject.GetComponent<TMP_Text>().text = "Distance " + distance.ToString();
        
    }
}
