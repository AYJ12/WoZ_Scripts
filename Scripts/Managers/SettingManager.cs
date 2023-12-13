using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingManager
{
    public AudioMixerGroup masterAudioMixerGroup;
    public float masterVolumeValue = 1f;
    public float bgmVolumeValue = 1f;
    public float effectVolumeValue = 1f;
    public Resolution resolution;
    public UnityEngine.Rendering.RenderPipelineAsset curRenderPipelinAssets;
    public float idleSensitivity = 300f;
    public float zoomSensitivity = 100f;
    public float brightValue;
    public FullScreenMode fullScreenMode;
    public void Init()
    {
    }

    //public void SetMouseIdelSen()
    //{
    //    idleSensitivity = _idleSlider.value * 100.0f;
    //}

    //public void SetMouseZoomSen()
    //{
    //    zoomSensitivity = _zoomSlider.value * 100.0f;
    //}

    //public void SetBright()
    //{
    //    brightValue = _brightSlider.value;
    //}
}
