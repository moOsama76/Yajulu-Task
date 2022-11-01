using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakAnimation : MonoBehaviour
{
    GameObject character;
    float forceMagnitude = 4000;
    void Start()
    {
        character = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(breakAnimation());
    }

    IEnumerator breakAnimation(){
        int gravityDirection = character.GetComponent<CharacterMovement>().gravityDirection;
        float dirX = Random.Range(-.1f, .1f), dirY = -Random.Range(0, .2f), dirZ = 1;
        GetComponent<Rigidbody>().AddForce(new Vector3(dirX, gravityDirection*dirY, dirZ).normalized*forceMagnitude);
        yield return new WaitForSeconds(2);
        if(transform.parent){
            Destroy(transform.parent.gameObject);
        }
    }
}
