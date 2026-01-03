using UnityEngine;
using UnityEngine.SceneManagement;

public class SecretSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject backToMenuButton;

    void Start()
    {
        // Ensure button is hidden at the start
        if (backToMenuButton != null) 
            backToMenuButton.SetActive(false);
    }

    void Update()
    {
        // If player clicks anywhere...
        if (Input.GetMouseButtonDown(0))
        {
            // ...reveal the button
            if (backToMenuButton != null)
                backToMenuButton.SetActive(true);
        }
    }

    // Assign this to the Button's OnClick event
    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0); // Index 0 is usually the Main Menu
    }
}