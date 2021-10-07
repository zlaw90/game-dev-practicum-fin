using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(AudioSource))]
public class VolumeControl : MonoBehaviour
{
    public GameSettings.SoundType SoundType = GameSettings.SoundType.Generic;
    private AudioSource _audio;
    private float initialVolume;
    // Start is called before the first frame update
    void Start()
    {
        _audio = GetComponent<AudioSource>();
        initialVolume = _audio.volume;
        GameSettings.VolumeSettingsWereChanged += UpdateVolume;
        UpdateVolume();
    }
    public void UpdateVolume()
    {
        float multiplier = GameSettings.MasterVolume * 
            (SoundType == GameSettings.SoundType.Music ? GameSettings.MusicVolume 
            : SoundType == GameSettings.SoundType.SoundEffect ? GameSettings.EffectVolume : 1f);
        _audio.volume = Mathf.Clamp(initialVolume * multiplier, 0f, 1f);
    }
    private void Update()
    {

    }
}
