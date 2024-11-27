using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BTNManager : MonoBehaviour
{
    public void OnClickPlay(string levelName)
    {
        SceneManager.LoadScene(levelName); // load the scene with chosen name
    }

    public void OnClickQuitGame()
    { 
        Application.Quit(); // exits the application
    }
}
