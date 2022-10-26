using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCloning : MonoBehaviour
{
    public GameObject character;
    float posZ;
    void Start()
    {
        character = GameObject.FindGameObjectWithTag("Player");
        InvokeRepeating("initiateEnemy", 2.0f, 0.5f);
    }

    void Update(){
        posZ = character.transform.position.z;
    }
    void initiateEnemy()
    {
        if(character.GetComponent<CharacterMovement>().playerAlive){
            float posX = Random.Range(-4f, 4f);
            GameObject copy = Instantiate(gameObject, new Vector3(posX, 0.7f, posZ+150), Quaternion.identity);
            Destroy(copy.GetComponent<EnemyCloning>());
            copy.tag = "enemyClone";
            copy.AddComponent<EnemyMechanics>();
            posX = Random.Range(-4f, 4f);
            copy = Instantiate(gameObject, new Vector3(posX, -2f, posZ+150), Quaternion.identity);
            Destroy(copy.GetComponent<EnemyCloning>());
            copy.tag = "enemyClone";
            copy.AddComponent<EnemyMechanics>();
        }
    }
}
