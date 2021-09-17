using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchChecker : MonoBehaviour
{
    public bool isDragging;

    Ray2D ray;
    RaycastHit2D hit;

    void Update()
    {
        if (Input.touchCount < 1) return;

        Touch touch = Input.GetTouch(0);
        // Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);

        // ray = new Ray2D(touchPos, Vector2.up); //Vector2.zero    
        hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.position), Vector2.zero, Mathf.Infinity);

        if (touch.phase == TouchPhase.Began)
        {
            print(hit.collider.gameObject.name);
            
            if (hit.rigidbody && hit.rigidbody.tag == "Block")
            {
                print("제대로 발견");
                Block.nowBlock = hit.rigidbody.gameObject.GetComponent<Block>();
            }
        }
        else if (touch.phase == TouchPhase.Moved)
        {
            if (Block.nowBlock == null) return;

            Block.nowBlock.DragBlock(hit.point);
            isDragging = true;
        }
        else if (touch.phase == TouchPhase.Ended)
        {
            if (Block.nowBlock == null) return;

            if (!isDragging)
                Block.nowBlock.RotateBlock(hit.point);

            Block.nowBlock.SetBlockToPos();
            isDragging = false;
        }


    }
}
