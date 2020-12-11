using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WalkThroughTigger : MonoBehaviour
{
    [SerializeField] UnityEvent OnWalkThrough;
    public bool isRepeatable = false;
    private bool already_done = false;
    public void OnTriggerEnter(Collider other)
    {
        if(!already_done)
        {
            OnWalkThrough.Invoke();
        }

        if(!isRepeatable)
        {
            already_done = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
