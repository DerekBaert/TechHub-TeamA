using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    [Tooltip("If true, trash will be knocked outward immediately on overlap. If false, left click triggers knock.")]
    public bool autoKnockOnOverlap = false;

    [Tooltip("Multiplier applied to trash moveSpeed when knocked outward.")]
    public float knockSpeedMultiplier = 1.2f;

    public string trashTag = "TrashObstacle";

    // Track overlapping objects using the base GameObject to support different scripts
    private readonly HashSet<GameObject> _overlappingObjects = new HashSet<GameObject>();

    [Tooltip("Clip to play ONLY when clicking while overlapping trash.")]
    public AudioClip clickSfx;

    public AudioSource clickAudioSource;
    public ParticleSystem knockParticleEffect;
    private AudioSource _internalAudioSource;

    private void Start()
    {
        _internalAudioSource = clickAudioSource != null ? clickAudioSource : GetComponent<AudioSource>();
        if (_internalAudioSource == null && clickSfx != null)
        {
            _internalAudioSource = gameObject.AddComponent<AudioSource>();
            _internalAudioSource.playOnAwake = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(trashTag)) return;
        
        _overlappingObjects.Add(other.gameObject);

        if (autoKnockOnOverlap)
        {
            ExecuteKnock(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (_overlappingObjects.Contains(other.gameObject))
        {
            _overlappingObjects.Remove(other.gameObject);
        }
    }

    private void Update()
    {
        // Only trigger if Left Click is pressed AND there are items in the overlap set
        if (Input.GetMouseButtonDown(0) && _overlappingObjects.Count > 0)
        {
            bool playedSound = false;

            foreach (GameObject obj in _overlappingObjects)
            {
                if (obj != null)
                {
                    ExecuteKnock(obj);
                    
                    // Play sound only once per click, even if hitting multiple items
                    if (!playedSound)
                    {
                        PlayInteractionSound();
                        playedSound = true;
                    }
                }
            }

            _overlappingObjects.Clear(); // Clear after knocking
            if (knockParticleEffect != null) knockParticleEffect.Play();
        }
    }

    private void ExecuteKnock(GameObject target)
{
    // Try Hander first
    if (target.TryGetComponent(out Hander h)) 
    {
        h.ReceiveClick(); 
    }
    // Try CigaretEel second
    else if (target.TryGetComponent(out CigaretEel e)) 
    {
        e.ReceiveClick(); 
    }
    // Try CartenCrab last
    else if (target.TryGetComponent(out CartenCrab c))
    {
        // Assuming you add ReceiveClick to CartenCrab too
        // c.ReceiveClick(); 
    }
}

    private void PlayInteractionSound()
    {
        if (clickSfx != null)
        {
            var src = clickAudioSource != null ? clickAudioSource : _internalAudioSource;
            if (src != null) src.PlayOneShot(clickSfx);
        }
    }
}