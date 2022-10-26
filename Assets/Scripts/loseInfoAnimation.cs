using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class loseInfoAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 5), 1f, false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
