using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchChecker : MonoBehaviour
{
    public static Vector2 touchPos;

    // Update is called once per frame
    // void Update()
    // {
    //     if(Input.GetTouch(0).phase == TouchPhase.Began)
    //     {
    //         Touch touch = Input.GetTouch(0);

    //         RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.position), Vector2.zero);

    //         if(hit.collider.tag == "Block")
    //         {
    //             hit.collider.GetComponent<Block>().Rotate();
    //         }
    //     }
    // }
}
