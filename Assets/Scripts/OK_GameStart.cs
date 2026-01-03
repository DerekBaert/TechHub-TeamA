using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OK_GameStart : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(2); // Load the scene with index 2 (Game Scene)
    }
    
}