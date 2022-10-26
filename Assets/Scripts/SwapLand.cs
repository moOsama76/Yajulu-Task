using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapLand : MonoBehaviour
{

    public GameObject theOtherLand, character, limit;
    public bool swapValid = true;
    void Update()
    {
        if(swapValid && character.transform.position.z > limit.transform.position.z){
            theOtherLand.transform.position = new Vector3(theOtherLand.transform.position.x, theOtherLand.transform.position.y, transform.position.z+100);
            swapValid = false;
            theOtherLand.GetComponent<SwapLand>().swapValid = true;
        }
    }
}
