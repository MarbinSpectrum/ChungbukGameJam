using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PopUp : MonoBehaviour
{
    Canvas canvas;
    GraphicRaycaster graphicRaycaster;
    PointerEventData pointerEventData;

    private void Start()
    {
        canvas = this.GetComponent<Canvas>();
        graphicRaycaster = canvas.GetComponent<GraphicRaycaster>();
        pointerEventData = new PointerEventData(null);
    }

    private void Update()
    {
        SearchElements();
    }

    private void SearchElements()
    {
        if (Input.GetMouseButtonDown(0))
        {
            pointerEventData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();

            graphicRaycaster.Raycast(pointerEventData, results);

            if (results.Count <= 0)
                canvas.gameObject.SetActive(false);
        }
    }
}
