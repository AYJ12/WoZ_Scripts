using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI_Setting : MonoBehaviour
{
    [SerializeField]
    private AudioMixerGroup audioMixer;

    // Dropdown UI
    [SerializeField]
    private TMP_Dropdown _dropdownResolution;
    [SerializeField]
    private TMP_Dropdown _dropdownGraphics;
    [SerializeField]
    private Slider _idleSlider;
    [SerializeField]
    private Slider _zoomSlider;
    [SerializeField]
    private Slider _brightSlider;
    [SerializeField]
    private Slider _masterVolumeSlider;
    [SerializeField]
    private Slider _bgmVolumeSlider;
    [SerializeField]
    private Slider _effectVolumeSlider;


    public List<Resolution> _resolutions = new List<Resolution>();
    private int _resolutionsIndex;
    [SerializeField]
    private List<UnityEngine.Rendering.RenderPipelineAsset> _renderPipelineAssets;
    public FullScreenMode fullScreenMode;
    public void Init()
    {
        Manager.Setting.masterAudioMixerGroup = audioMixer;

        _resolutions.AddRange(Screen.resolutions);
        _dropdownResolution.ClearOptions();

        foreach (Resolution resolution in _resolutions)
        {
            TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData();
            optionData.text = resolution.width + " X " + resolution.height;
            _dropdownResolution.options.Add(optionData);
        }

        _dropdownResolution.RefreshShownValue();
        fullScreenMode = FullScreenMode.FullScreenWindow;
        _idleSlider.value = Manager.Setting.idleSensitivity * 0.01f;
        _zoomSlider.value = Manager.Setting.zoomSensitivity * 0.01f;
        _masterVolumeSlider.value = Manager.Setting.masterVolumeValue;
        _bgmVolumeSlider.value = Manager.Setting.bgmVolumeValue;
        _effectVolumeSlider.value = Manager.Setting.effectVolumeValue;
    }

    private void Start()
    {
        Init();
    }

    public void ChangeScreenOption(int idx)
    {
        _resolutionsIndex = idx;
    }

    public void SetVolume()
    {
        Manager.Setting.masterVolumeValue = _masterVolumeSlider.value;
        Manager.Setting.bgmVolumeValue = _bgmVolumeSlider.value;
        Manager.Setting.effectVolumeValue = _effectVolumeSlider.value;
        Manager.Setting.masterAudioMixerGroup.audioMixer.SetFloat("Master", Manager.Setting.masterVolumeValue);
        Manager.Setting.masterAudioMixerGroup.audioMixer.SetFloat(Define.Sound.Bgm.ToString(), Manager.Setting.bgmVolumeValue);
        Manager.Setting.masterAudioMixerGroup.audioMixer.SetFloat(Define.Sound.Effect.ToString(), Manager.Setting.effectVolumeValue);
    }

    public void SetScreen()
    {
        Screen.SetResolution(_resolutions[_resolutionsIndex].width, _resolutions[_resolutionsIndex].height
            , fullScreenMode, _resolutions[_resolutionsIndex].refreshRateRatio);
        Manager.Setting.resolution = _resolutions[_resolutionsIndex];
    }

    public void FullScreenSet(bool isFull)
    {
        fullScreenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }

    public void SetPipeline()
    {
        int value = _dropdownGraphics.value;
        QualitySettings.renderPipeline = _renderPipelineAssets[value];
        Manager.Setting.curRenderPipelinAssets = _renderPipelineAssets[value];
    }

    public void SetMouseIdelSen()
    {
        Manager.Setting.idleSensitivity = _idleSlider.value * 100.0f;
    }

    public void SetMouseZoomSen()
    {
        Manager.Setting.zoomSensitivity = _zoomSlider.value * 100.0f;
    }

    public void SetBright()
    {
        Manager.Setting.brightValue = _brightSlider.value;
    }
}
