/************************************
 * Author: Emmett Hale
 * 
 * Purpose: Handler script for settings
 * menu
 ************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsHandler : MonoBehaviour
{
    public AudioMixer mixer;
    // Start is called before the first frame update
    void Start()
    {
        //Load Settings
    }

    public void SetVolume(float value)
    {
        mixer.SetFloat("volume", value);
    }

    public void SetSensitivity(float value)
    {

    }

    public void SetRenderDistance(float value)
    {

    }
}
