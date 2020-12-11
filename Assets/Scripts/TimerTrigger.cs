using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimerTrigger : MonoBehaviour
{
    [SerializeField] UnityEvent OnTimerStart;
    [SerializeField] UnityEvent OnTimerFinish;

    private bool timerOn = false;

    public float timerDuration = 1f;

    private float currentTimePassed = 0;

    // Update is called once per frame
    void Update()
    {
        if(timerOn)
        {
            currentTimePassed += Time.deltaTime;

            if(currentTimePassed >= timerDuration)
            {
                timerOn = false;
                currentTimePassed = 0;
                OnTimerFinish.Invoke();
            }
        }
    }

    public void StartTimer()
    {
        timerOn = true;
        currentTimePassed = 0;
        OnTimerStart.Invoke();
    }
}
