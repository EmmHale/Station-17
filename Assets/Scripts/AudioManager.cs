/************************************
 * Author: Emmett Hale
 * 
 * Purpose: Manages game sounds
 * 
 * Does not work for spacial sounds
 ************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] Sounds;

    public static AudioManager instance;
    
    // Start is called before the first frame update
    void Awake()
    {
        //Enable Singleton
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        //Mark to not destroy
        DontDestroyOnLoad(gameObject);

        //Make sources for sounds
        foreach(Sound sound in Sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;

            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;  
            sound.source.loop = sound.loop;
            sound.source.playOnAwake = false;
            sound.source.outputAudioMixerGroup = sound.group;
        }
    }

    //Plays a sound with the passed name
    public void Play(string sound_name)
    {
        Sound s = Array.Find(Sounds, sound => sound.name == sound_name);

        if (s != null)
        {
            s.source.Play();
        }
        else
        {
            Debug.Log("Sound name not found!");
        }
    }

    //Stops a sound with the passed name
    public void Stop(string sound_name)
    {
        Sound s = Array.Find(Sounds, sound => sound.name == sound_name);

        if (s != null)
        {
            s.source.Stop();
        }
        else
        {
            Debug.Log("Sound name not found!");
        }
    }

    //Stops all sounds
    public void StopAll()
    {
        foreach (Sound sound in Sounds)
        {
            if (sound != null)
            {
                sound.source.Stop();
            }
            else
            {
                Debug.Log("Sound name not found!");
            }
        }
    }
}
