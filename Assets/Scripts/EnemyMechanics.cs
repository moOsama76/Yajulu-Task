using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMechanics : MonoBehaviour
{
    GameObject character;
    
    void Start(){
        character = GameObject.FindGameObjectWithTag("Player");
    }

    void Update(){
        if(transform.position.z < character.transform.position.z-5)
            Destroy(gameObject);   
    }
}
