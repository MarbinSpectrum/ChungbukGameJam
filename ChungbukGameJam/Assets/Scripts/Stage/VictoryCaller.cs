using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryCaller : MonoBehaviour
{
    public Canvas winCanvas;
    public delegate void VictoryDele();
    public event VictoryDele CallVictory;

    // Start is called before the first frame update
    void Start()
    {
        winCanvas.gameObject.SetActive(false);
        CallVictory += ActivateWinCanvas;
    }

    void ActivateWinCanvas()
    {

    }

    public void InvokeCallVictory()
    {
        CallVictory?.Invoke();
    }
}
