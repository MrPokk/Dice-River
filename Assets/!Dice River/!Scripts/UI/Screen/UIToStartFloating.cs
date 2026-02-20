using UINotDependence.Core;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class UIToStartFloating : UIScreen
{
    [Header("UI Elements")]
    public Toggle toggleMusic;
    public Toggle toggleSfx;

    [Header("Audio")]
    public AudioMixer mixer;

    private const string MIXER_MUSIC = "MusicVolume";
    private const string MIXER_SFX = "SfxVolume";

    private const string PREF_MUSIC_ON = "MusicEnabled";
    private const string PREF_SFX_ON = "SfxEnabled";

    public override void Open()
    {
        base.Open();

        var isMusicOn = PlayerPrefs.GetInt(PREF_MUSIC_ON, 1) == 1;
        var isSfxOn = PlayerPrefs.GetInt(PREF_SFX_ON, 1) == 1;

        toggleMusic.isOn = isMusicOn;
        toggleSfx.isOn = isSfxOn;

        SetVolume(MIXER_MUSIC, isMusicOn);
        SetVolume(MIXER_SFX, isSfxOn);

        toggleMusic.onValueChanged.AddListener(OnMusicToggle);
        toggleSfx.onValueChanged.AddListener(OnSfxToggle);
    }

    public override void Close()
    {
        toggleMusic.onValueChanged.RemoveAllListeners();
        toggleSfx.onValueChanged.RemoveAllListeners();
        base.Close();
    }

    private void OnMusicToggle(bool isOn)
    {
        SetVolume(MIXER_MUSIC, isOn);
        PlayerPrefs.SetInt(PREF_MUSIC_ON, isOn ? 1 : 0);
    }

    private void OnSfxToggle(bool isOn)
    {
        SetVolume(MIXER_SFX, isOn);
        PlayerPrefs.SetInt(PREF_SFX_ON, isOn ? 1 : 0);
    }

    private void SetVolume(string parameter, bool isOn)
    {
        var targetVolume = isOn ? 0f : -80f;
        mixer.SetFloat(parameter, targetVolume);
    }
}
