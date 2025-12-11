using UnityEngine;
public class DeathPanelBGM : MonoBehaviour
{
    [Tooltip("Reference to the Death Panel GameObject (the panel that becomes active on death).")]
    public GameObject deathPanel;

    [Tooltip("AudioSource that plays the background music. If left empty, the script will try to find a suitable AudioSource in the scene. If none found and fallbackToListener is true, it will mute the global AudioListener volume.")]
    public AudioSource bgmSource;

    [Tooltip("If true and no AudioSource is found, the script will mute the global AudioListener.volume instead.")]
    public bool fallbackToListener = true;

    // internal bookkeeping
    private bool _lastPanelState = false;
    private bool _wasBgmPlayingBeforeMute = false;
    private float _previousListenerVolume = 1f;
    private bool _listenerMutedByThis = false;

    void Start()
    {
        // try to locate the deathPanel if not assigned
        if (deathPanel == null)
        {
            var found = GameObject.Find("DeathPanel");
            if (found != null) deathPanel = found;
        }

        // if bgmSource not assigned, try to find a sensible AudioSource (prefer looping sources)
        if (bgmSource == null)
        {
            var all = FindObjectsOfType<AudioSource>();
            AudioSource loopCandidate = null;
            foreach (var s in all)
            {
                if (s == null) continue;
                if (s.loop)
                {
                    loopCandidate = s;
                    break;
                }
            }

            if (loopCandidate != null) bgmSource = loopCandidate;
            else if (all.Length > 0) bgmSource = all[0];
        }

        // initial state
        _lastPanelState = deathPanel != null && deathPanel.activeSelf;
        ApplyDeathPanelState(_lastPanelState);
    }

    void Update()
    {
        if (deathPanel == null) return;

        bool active = deathPanel.activeSelf;
        if (active != _lastPanelState)
        {
            ApplyDeathPanelState(active);
            _lastPanelState = active;
        }
    }

    private void ApplyDeathPanelState(bool panelActive)
    {
        if (panelActive)
        {
            // mute/stop BGM
            if (bgmSource != null)
            {
                _wasBgmPlayingBeforeMute = bgmSource.isPlaying;
                if (bgmSource.isPlaying)
                {
                    bgmSource.Pause();
                }
            }
            else if (fallbackToListener)
            {
                _previousListenerVolume = AudioListener.volume;
                AudioListener.volume = 0f;
                _listenerMutedByThis = true;
            }
        }
        else
        {
            // restore BGM
            if (bgmSource != null)
            {
                if (_wasBgmPlayingBeforeMute)
                {
                    bgmSource.UnPause();
                }
            }
            else if (fallbackToListener && _listenerMutedByThis)
            {
                AudioListener.volume = _previousListenerVolume;
                _listenerMutedByThis = false;
            }
        }
    }
}
