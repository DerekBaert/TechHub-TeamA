using UnityEngine;
using UnityEngine.UI; // Required for the Image component
using UnityEngine.SceneManagement;

public class SecretLevelTrigger : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int requiredClicks = 10;
    [SerializeField] private float timeLimit = 3f;
    [SerializeField] private int secretSceneIndex = 3;

    [Header("Visual Feedback")]
    [SerializeField] private Color warningColor = Color.red;
    [SerializeField] private float shakeIntensity = 10f;

    private int _clickCount = 0;
    private float _timer = 0f;
    private Vector3 _originalPos;
    private Color _originalColor;
    
    private Image _imageComponent; // Changed from Text to Image
    private RectTransform _rectTransform;

    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _imageComponent = GetComponent<Image>(); // Look for the Image component

        if (_imageComponent == null)
        {
            Debug.LogError("No Image component found on " + gameObject.name + ". Make sure this script is on your Title Image!");
        }
        else
        {
            _originalColor = _imageComponent.color;
        }

        _originalPos = _rectTransform.localPosition;
    }

    void Update()
    {
        if (_clickCount > 0)
        {
            _timer += Time.deltaTime;

            // Apply Shake Effect
            if (_clickCount < requiredClicks)
            {
                float progress = (float)_clickCount / requiredClicks;
                
                // Position Shake
                float currentShake = progress * shakeIntensity;
                _rectTransform.localPosition = _originalPos + (Vector3)Random.insideUnitCircle * currentShake;
                
                // Color Tinting
                if (_imageComponent != null)
                {
                    _imageComponent.color = Color.Lerp(_originalColor, warningColor, progress);
                }
            }

            // Reset if time runs out
            if (_timer > timeLimit)
            {
                ResetSecret();
            }
        }
    }

    public void OnTitleClicked()
    {
        _clickCount++;
        _timer = 0f; 

        if (_clickCount >= requiredClicks)
        {
            SceneManager.LoadScene(secretSceneIndex);
        }
    }

    private void ResetSecret()
    {
        _clickCount = 0;
        _timer = 0f;
        _rectTransform.localPosition = _originalPos;
        if (_imageComponent != null) _imageComponent.color = _originalColor;
    }
}