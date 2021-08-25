using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryCaller : MonoBehaviour
{
    public Canvas winCanvas;

    public delegate void VictoryCanvasDele(bool b);
    public static event VictoryCanvasDele WinEvent; 

    // Start is called before the first frame update
    void Start()
    {
        winCanvas.gameObject.SetActive(false);

        WinEvent += ActivateWinCanvas;
    }

    public void ActivateWinCanvas(bool isVictory) => winCanvas.gameObject.SetActive(isVictory);

    public static void InvokeWinEvent(bool isVictory) => WinEvent?.Invoke(isVictory);
}
