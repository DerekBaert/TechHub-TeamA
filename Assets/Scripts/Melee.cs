using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    [Tooltip("If true, trash will be knocked outward immediately on overlap. If false, left click triggers knock.")]
    public bool autoKnockOnOverlap = false;

    [Tooltip("Multiplier applied to trash moveSpeed when knocked outward.")]
    public float knockSpeedMultiplier = 1.2f;

    // Optional tag filter (set on trash prefabs)
    public string trashTag = "TrashObstacle";

    // track overlapping Hander instances
    private readonly HashSet<Hander> _overlapping = new HashSet<Hander>();

    [Tooltip("Optional clip to play when the mouse is clicked (left button).")]
    public AudioClip clickSfx;

    [Tooltip("Optional AudioSource to play the click SFX. If null, one will be created on Start.")]
    public AudioSource clickAudioSource;

    private AudioSource _audioSource;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(trashTag)) return;

        var h = other.GetComponent<Hander>();
        if (h == null) return;

        _overlapping.Add(h);

        if (autoKnockOnOverlap)
        {
            h.SendOutward(knockSpeedMultiplier);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var h = other.GetComponent<Hander>();
        if (h != null) _overlapping.Remove(h);
    }

    private void Update()
    {
        // play click SFX on left mouse click (regardless of overlap)
        if (Input.GetMouseButtonDown(0))
        {
            if (clickSfx != null)
            {
                // prefer assigned source, fall back to the one we obtained/created
                var src = clickAudioSource != null ? clickAudioSource : _audioSource;
                if (src != null) src.PlayOneShot(clickSfx);
            }

            // left mouse click also triggers knock on all overlapping trash
            if (_overlapping.Count > 0)
            {
                foreach (var h in _overlapping)
                {
                    if (h != null) h.SendOutward(knockSpeedMultiplier);
                }
                _overlapping.Clear();
            }
        }
    }

    private void Start()
    {
        // resolve or create an AudioSource to play click SFX if needed
        _audioSource = clickAudioSource != null ? clickAudioSource : GetComponent<AudioSource>();
        if (_audioSource == null && clickSfx != null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.playOnAwake = false;
        }
    }
}
