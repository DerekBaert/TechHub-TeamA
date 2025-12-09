using UnityEngine;

public class MuteMusic : MonoBehaviour
{   
    [Tooltip("Volume to restore to when unmuting if previous volume is 0.")]
    [Range(0f, 1f)]
    public float defaultUnmutedVolume = 1f;

    // internal storage of previous (pre-mute) volume
    private float _previousVolume = 1f;
    private bool _isMuted = false;

    private void Start()
    {
        // capture current global volume on start as the default previous volume
        _previousVolume = Mathf.Clamp01(AudioListener.volume);
        _isMuted = _previousVolume <= 0f;
    }

    // Call with the UI Toggle's isOn value: when true => mute, when false => unmute
    public void MuteToggle(bool muteOn)
    {
        if (muteOn)
        {
            if (!_isMuted)
            {
                _previousVolume = Mathf.Clamp01(AudioListener.volume);
            }

            AudioListener.volume = 0f;
            _isMuted = true;
        }
        else
        {
            // restore previous volume, but if it was 0 use configured default
            var restore = _previousVolume > 0f ? _previousVolume : defaultUnmutedVolume;
            AudioListener.volume = Mathf.Clamp01(restore);
            _isMuted = AudioListener.volume <= 0f;
        }
    }
}
