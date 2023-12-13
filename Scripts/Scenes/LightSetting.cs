using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSetting : MonoBehaviour
{
    private float _lightValue;
    private Light _light;
    // Start is called before the first frame update
    void Start()
    {
        _light = GetComponent<Light>();
        _lightValue = Manager.Setting.brightValue;
        _light.intensity *= _lightValue;
    }
}
