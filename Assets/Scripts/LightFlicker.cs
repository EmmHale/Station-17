/*************************************
 * Author: Emmett Hale
 * 
 * Purpose: Light flickering managment script
 *************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    Light Light;
    public bool BlinkingEnabled = true;
    public bool FadingEnabled = true;

    [Tooltip("AudioSource for sound. Leave null for no sound")]
    public AudioSource source;

    [Tooltip("Chance to proc out from 0 to 10(0 = garenteed, 10 = never)")]
    public float fadingProc = 7;

    [Tooltip("Chance to proc out from 0 to 10(0 = garenteed, 10 = never)")]
    public float blinkingProc = 8;

    [Tooltip("Seconds interval till proc")]
    public float timeToRoll = .5f;

    // Start is called before the first frame update
    void Start()
    {
        Light = GetComponent<Light>();
        fadingBaseRange = Light.range;
        blinkBaseIntensity = Light.intensity;
    }

    bool fading = false;
    float fadingDurationToPass = 0;
    float fadingDirection = 0;
    float fadingBaseRange = 0;

    const float fadingBaseTime = 3f;

    bool blinking = false;
    float blinkingDurationToPass = 0;
    float blinkBaseIntensity = 0;
    float timeSinceRoll = 0;

    // Update is called once per frame
    void Update()
    {
        if (timeSinceRoll >= timeToRoll)
        {
            float rollBlinking = Random.Range(0.0f, 10f);
            float rollFading = Random.Range(0.0f, 10f);

            if(rollBlinking >= blinkingProc && !blinking && BlinkingEnabled)
            {
                blinking = true;
                blinkingDurationToPass = .25f;
            }

            if (rollFading >= fadingProc && !fading && FadingEnabled)
            {
                fading = true;
                fadingDirection = -1;
                fadingDurationToPass = fadingBaseTime;
            }

            timeSinceRoll = 0;
        }
        else
        {
            timeSinceRoll += Time.deltaTime;
        }


        if(blinking)
        {
            if(blinkingDurationToPass <= 0)
            {
                Light.intensity = blinkBaseIntensity;
                blinking = false;
                source.Play();
            }
            else
            {
                Light.intensity = 0;
                source.Stop();
            }

            blinkingDurationToPass -= Time.deltaTime;
        }

        if(fading)
        {
            if(fadingDurationToPass <= fadingBaseTime / 2 && fadingDirection == -1)
            {
                fadingDirection = 1;
            }
            else if(fadingDurationToPass <= 0)
            {
                fading = false;

                Light.range = fadingBaseRange;
            }
            else
            {
                if(fadingDirection == -1)
                {
                    Light.range -= (fadingBaseRange / 10) * Time.deltaTime * 10;
                }
                else
                {
                    Light.range += (fadingBaseRange / 10) * Time.deltaTime * 10;
                }
            }

            fadingDurationToPass -= Time.deltaTime;
        }
    }
}
