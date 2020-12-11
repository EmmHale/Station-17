/****************************************
 * Purpose: Acts as a data class for 
 * Audio Manager
 * 
 ****************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public AudioClip clip;

    public string name;

    [Range(0f, 1f)]
    public float volume;

    [Range(0.1f, 3f)]
    public float pitch;

    public bool loop;
    
    [HideInInspector]
    public AudioSource source;

    public AudioMixerGroup group;
}

