using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchChecker : MonoBehaviour
{
    public static bool isDragging = false;

    Ray2D ray;
    public static RaycastHit2D hit;

    Touch touch;

    public Vector2 mousePositionForBlock;

    void Update()
    {
        if (Input.touchCount < 1)
        {
            if (EventSystem.current.IsPointerOverGameObject() == false)
            {
                ray = new Ray2D(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

                if (Input.GetMouseButtonDown(0))
                { 
                    isDragging = false;

                    if (hit.rigidbody && hit.rigidbody.tag == "Block")
                    {
                        Block.nowBlock = hit.rigidbody.gameObject.GetComponent<Block>();
                        Block.nowBlock.TouchBlock();
                        mousePositionForBlock = Block.nowBlock.transform.position;
                    }
                }

                if (Block.nowBlock == null) return;

                if (Input.GetMouseButtonUp(0))
                {
                    Block.nowBlock.SetBlockToPos();

                    if (Vector2.Distance(mousePositionForBlock, Block.nowBlock.transform.position) < 0.01f && !isDragging)
                        Block.nowBlock.RotateBlock(hit.point);  

                    Block.nowBlock = null;
                    mousePositionForBlock = Vector2.zero;
                }
            }
        }
        else
        {
            touch = Input.GetTouch(0);
            hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.position), Vector2.zero, Mathf.Infinity);

            if (EventSystem.current.IsPointerOverGameObject() == false)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    print(hit.collider.gameObject.name);

                    if (hit.rigidbody && hit.rigidbody.tag == "Block")
                    {
                        Block.nowBlock = hit.rigidbody.gameObject.GetComponent<Block>();
                        Block.nowBlock.TouchBlock();
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

                    Block.nowBlock.SetBlockToPos();
                    if (!isDragging)
                        Block.nowBlock.RotateBlock(hit.point);
                    
                    SettingCheckDelegate.CallInvoke(true);

                    Block.nowBlock = null;
                    isDragging = false;
                }
            }
        }
    }
}
