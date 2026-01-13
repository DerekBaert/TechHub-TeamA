using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Quit : MonoBehaviour
{

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void QuitGame()
{
    Debug.Log("Quit Game Pressed"); // This will show in Console
    Application.Quit(); // This will close the actual game build
    
    // If you want it to stop the play mode in the Editor:
    #if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
    #endif
}
}
   