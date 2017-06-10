using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIControl : MonoBehaviour {

    public void OnClickStart()
    {
        SceneManager.LoadScene("SetupScene");
    }

    public void OnClickQuit()
    {
        Application.Quit();
    }
}
