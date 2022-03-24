using PlayerPreferences;
using UnityEngine;
using UnityEngine.UI;

public class SetVolume : MonoBehaviour
{
    [SerializeField] private Music _Music;
    [SerializeField] private DataController _Data;
    //Sliders
    [SerializeField] private Slider _MasterSlider;
    [SerializeField] private Slider _SFXSlider;
    [SerializeField] private Slider _AmbianceSlider;
    [SerializeField] private Slider _MusicSlider;

    private void Start()
    {
        _MasterSlider.value = _Data.masterVolume;
        _SFXSlider.value = _Data.sfxVolume;
        _AmbianceSlider.value = _Data.ambianceVolume;
        _MusicSlider.value = _Data.musicVolume;
    }

    public void SetMasterVolume(float slider)
    {
        var val = 0f;
        if (slider >= 1) val = Mathf.Log10(slider / 100) * 20;
        else val = -80f;
        _Music.masterVolume = val;
        _Data.masterVolume = slider;
    }
    public void SetMusicVolume(float slider)
    {
        var val = 0f;
        if (slider >= 1) val = Mathf.Log10(slider / 100) * 20;
        else val = -80f;
        _Music.musicVolume = val;
        _Data.musicVolume = slider;
    }
    public void SetAmbianceVolume(float slider)
    {
        var val = 0f;
        if (slider >= 1) val = Mathf.Log10(slider / 100) * 20;
        else val = -80f;
        _Music.ambianceVolume = val;
        _Data.ambianceVolume = slider;
    }
    public void SetSFXVolume(float slider)
    {
        var val = 0f;
        if (slider >= 1) val = Mathf.Log10(slider / 100) * 20;
        else val = -80f;
        _Music.sfxVolume = val;
        _Data.sfxVolume = slider;
    }

    private void OnApplicationQuit()
    {
        _Data.SetPlayerData();
    }
}
