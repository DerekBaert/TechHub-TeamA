using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class StoryManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image displayImage;
    [SerializeField] private CanvasGroup panelAlpha;
    [SerializeField] private GameObject okButton;

    [Header("Sequence Assets")]
    [SerializeField] private Sprite[] storySprites; // 0 & 1: Story, 2: Tutorial
    [SerializeField] private float fadeDuration = 0.8f;
    [SerializeField] private float okButtonDelay = 2.0f;

    private int _currentIndex = 0;
    private bool _isFading = false;
    private bool _sequenceFinished = false;

    void Start()
    {
        // Setup initial state
        displayImage.sprite = storySprites[0];
        panelAlpha.alpha = 1;
        okButton.SetActive(false);
    }

    void Update()
    {
        // Advance story on mouse click
        // Only works if we aren't fading, and haven't reached the tutorial yet
        if (Input.GetMouseButtonDown(0) && !_isFading && !_sequenceFinished)
        {
            if (_currentIndex < storySprites.Length - 1)
            {
                StartCoroutine(TransitionToNext());
            }
        }
    }

    private IEnumerator TransitionToNext()
    {
        _isFading = true;

        // 1. Fade out the current image
        yield return StartCoroutine(Fade(1, 0));

        // 2. Switch the image to the next one in the array
        _currentIndex++;
        displayImage.sprite = storySprites[_currentIndex];

        // 3. Fade the new image in
        yield return StartCoroutine(Fade(0, 1));
        
        _isFading = false;

        // 4. Check if we just landed on the Tutorial screen (the last index)
        if (_currentIndex == storySprites.Length - 1)
        {
            _sequenceFinished = true; // Stop the manual clicking logic
            StartCoroutine(ShowOkButtonWithDelay());
        }
    }

    private IEnumerator ShowOkButtonWithDelay()
    {
        // Wait for the specific delay (e.g., 2 seconds)
        yield return new WaitForSeconds(okButtonDelay);
        
        // Show the OK button so the player can start the game
        okButton.SetActive(true);
    }

    private IEnumerator Fade(float start, float end)
    {
        float elapsed = 0;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            panelAlpha.alpha = Mathf.Lerp(start, end, elapsed / fadeDuration);
            yield return null;
        }
        panelAlpha.alpha = end;
    }

    // Assign this to your OK button's OnClick() event in the Inspector
    public void LoadMainGame()
    {
        SceneManager.LoadSceneAsync(2); // Load the main game scene (index 2)
    }
}