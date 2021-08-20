using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        DontDestroyOnLoad(this);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void AppearCanvas(GameObject canvas)
    {
        if(canvas)
            canvas.SetActive(true);
    }
}
