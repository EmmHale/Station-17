using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ZoneTrigger : MonoBehaviour
{

    [SerializeField] UnityEvent OnEnter;
    [SerializeField] UnityEvent OnExit;

    public void OnTriggerEnter(Collider other)
    {
        OnEnter.Invoke();
    }

    public void OnTriggerExit(Collider other)
    {
        OnExit.Invoke();
    }
}
