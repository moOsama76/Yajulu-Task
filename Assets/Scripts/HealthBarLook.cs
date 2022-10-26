using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarLook : MonoBehaviour
{
    public Camera camera;
    void Start(){
        GetComponent<RectTransform>().sizeDelta = new Vector2(100, 1);
    }

}
