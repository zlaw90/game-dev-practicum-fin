using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public static class GameSettings
{
    public enum SoundType { Generic, Music, SoundEffect }
    public static event Action VolumeSettingsWereChanged;
    private static float? _masterVolume = null;
    public static float MasterVolume
    {
        get
        {
            if (_masterVolume.HasValue)
                return _masterVolume.Value;
            else
            {
                _masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
                return _masterVolume.Value;
            }
        }
        set
        {
            _masterVolume = value;
            PlayerPrefs.SetFloat("MasterVolume", value);
            VolumeSettingsWereChanged?.Invoke();
        }
    }
    private static float? _musicVolume = null;
    public static float MusicVolume
    {
        get
        {
            if (_musicVolume.HasValue)
                return _musicVolume.Value;
            else
            {
                _musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
                return _musicVolume.Value;
            }
        }
        set
        {
            _musicVolume = value;
            PlayerPrefs.SetFloat("MusicVolume", value);
            VolumeSettingsWereChanged?.Invoke();
        }
    }
    private static float? _effectVolume = null;
    public static float EffectVolume
    {
        get
        {
            if (_effectVolume.HasValue)
                return _effectVolume.Value;
            else
            {
                _effectVolume = PlayerPrefs.GetFloat("EffectVolume", 1f);
                return _effectVolume.Value;
            }
        }
        set
        {
            _effectVolume = value;
            PlayerPrefs.SetFloat("EffectVolume", value);
            VolumeSettingsWereChanged?.Invoke();
        }
    }
}

