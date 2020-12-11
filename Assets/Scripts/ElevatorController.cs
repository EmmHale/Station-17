/************************************
 * Author: Emmett Hale
 * 
 * Purpose: Big messy elevator script
 * 
 * 
 * ->Will rewrite when adding more 
 * scenes
 ************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ElevatorController : MonoBehaviour
{
    enum ElevatorState { GoingDown, GoingUp, Stopped};

    public GameObject elevator;

    public Transform upperLimit;

    public Transform lowerLimit;

    Vector3 newPosition;

    ElevatorState state;

    [SerializeField] UnityEvent OnElevatorStartUp;
    [SerializeField] UnityEvent OnElevatorStopUp;

    [SerializeField] UnityEvent OnElevatorStartDown;
    [SerializeField] UnityEvent OnElevatorStopDown;

    [SerializeField] UnityEvent OnElevatorMovingUp;

    [SerializeField] UnityEvent OnElevatorMovingDown;

    public float movingTriggerRate = 10f;

    private float timeSinceMovingTrigger = 0;

    public List<GameObject> doorsUp;
    public List<GameObject> doorsDown;

    public float smooth = 0;
    public float upperSoundPitch = 1;
    public float lowerSoundPitch = 1;

    public float startMovingTime = 3f;
    private float timeSinceStart = 0;

    public bool isTeleport = true;
    public float teleportTime = 5f;
    private float timeToTeleport = 0;

    public AudioSource elevatorNoise;
    // Start is called before the first frame update
    void Start()
    {
        state = ElevatorState.Stopped;
    }

    private Vector3 velocity = Vector3.zero;
    // Update is called once per frame
    void Update()
    {
        if (timeSinceStart >= startMovingTime)
        {
            if (!isTeleport)
            {
                if (state == ElevatorState.GoingDown)
                {
                    newPosition = lowerLimit.position;
                    transform.position = Vector3.Lerp(transform.position, newPosition, smooth * Time.deltaTime);

                    if (Vector3.Distance(transform.position, newPosition) <= 0.1)
                    {
                        transform.position = lowerLimit.position;
                        state = ElevatorState.Stopped;
                        OnElevatorStopDown.Invoke();
                    }
                    else
                    {
                        timeSinceMovingTrigger += Time.deltaTime;

                        if (timeSinceMovingTrigger >= movingTriggerRate)
                        {
                            OnElevatorMovingDown.Invoke();
                            timeSinceMovingTrigger = 0;
                        }
                    }
                }
                else if (state == ElevatorState.GoingUp)
                {
                    newPosition = upperLimit.position;
                    transform.position = Vector3.Lerp(transform.position, newPosition, smooth * Time.deltaTime);

                    if (Vector3.Distance(transform.position, newPosition) <= 0.1)
                    {
                        transform.position = upperLimit.position;
                        state = ElevatorState.Stopped;
                        OnElevatorStopUp.Invoke();
                    }
                    else
                    {
                        timeSinceMovingTrigger += Time.deltaTime;

                        if (timeSinceMovingTrigger >= movingTriggerRate)
                        {
                            OnElevatorMovingUp.Invoke();
                            timeSinceMovingTrigger = 0;
                        }
                    }
                }
            }
            else
            {
                if (timeToTeleport >= teleportTime)
                {
                    if (state == ElevatorState.GoingDown)
                    {
                        elevator.transform.position = lowerLimit.position;

                        if(PlayerMovement.instance != null)
                        {
                            PlayerMovement.instance.transform.position = new Vector3(0, 0, 0);
                        }

                        state = ElevatorState.Stopped;
                        OnElevatorStopDown.Invoke();
                    }
                    else if (state == ElevatorState.GoingUp)
                    {
                        elevator.transform.position = upperLimit.position;

                        if(PlayerMovement.instance != null)
                        {
                            PlayerMovement.instance.transform.position = new Vector3(0, 0, 0);
                        }

                        state = ElevatorState.Stopped;
                        OnElevatorStopUp.Invoke();
                    }
                }
                else
                {
                    timeToTeleport += Time.deltaTime;

                    if (state == ElevatorState.GoingDown)
                    {
                        OnElevatorMovingDown.Invoke();
                    }
                    else if (state == ElevatorState.GoingUp)
                    {
                        OnElevatorMovingUp.Invoke();
                    }
                }
            }
        }
        else
        {
            timeSinceStart += Time.deltaTime;
        }
    }

    public void ElevatorNoise()
    {
        elevatorNoise.Stop();

        elevatorNoise.pitch = Mathf.Lerp(elevatorNoise.pitch, lowerSoundPitch, Time.deltaTime);

        elevatorNoise.Play();
    }

    public void OpenDoorsUp()
    {
        foreach(GameObject door in doorsUp)
        {
            door.GetComponent<Animator>().SetBool("IsClosed", false);
            door.GetComponent<Animator>().SetBool("IsOpen", true);
        }

        elevatorNoise.pitch = lowerSoundPitch;
        elevatorNoise.Stop();
        elevatorNoise.Play();
    }

    public void CloseDoorsUp()
    {
        foreach (GameObject door in doorsUp)
        {
            door.GetComponent<Animator>().SetBool("IsClosed", true);
            door.GetComponent<Animator>().SetBool("IsOpen", false);
        }

        elevatorNoise.pitch = lowerSoundPitch;
        elevatorNoise.Stop();
        elevatorNoise.Play();
        elevatorNoise.pitch = upperSoundPitch;
    }

    public void OpenDoorsDown()
    {
        foreach (GameObject door in doorsDown)
        {
            door.GetComponent<Animator>().SetBool("IsClosed", false);
            door.GetComponent<Animator>().SetBool("IsOpen", true);
        }

        elevatorNoise.pitch = lowerSoundPitch;
        elevatorNoise.Stop();
        elevatorNoise.Play();
    }

    public void CloseDoorsDown()
    {
        foreach (GameObject door in doorsDown)
        {
            door.GetComponent<Animator>().SetBool("IsClosed", true);
            door.GetComponent<Animator>().SetBool("IsOpen", false);
        }

        elevatorNoise.pitch = lowerSoundPitch;
        elevatorNoise.Stop();
        elevatorNoise.Play();
        elevatorNoise.pitch = upperSoundPitch;
    }

    private void OnTriggerEnter(Collider other)
    {
            Debug.Log("Elevator Enter Tigger");
            other.transform.parent = gameObject.transform;
    }

    private void OnTriggerExit(Collider other)
    {
       
        Debug.Log("Elevator Exit Tigger");
        other.transform.parent = null;
    }

    public void GoUp()
    {
        if(state == ElevatorState.Stopped)
        {
            state = ElevatorState.GoingUp;
            timeSinceMovingTrigger = 0;
            OnElevatorStartUp.Invoke();
            timeSinceStart = 0;
            timeToTeleport = 0;

            elevatorNoise.pitch = upperSoundPitch;
        }
    }

    public void GoDown()
    {
        if (state == ElevatorState.Stopped)
        {
            state = ElevatorState.GoingDown;
            timeSinceMovingTrigger = 0;
            OnElevatorStartDown.Invoke();
            timeSinceStart = 0;
            timeToTeleport = 0;

            elevatorNoise.pitch = upperSoundPitch;
        }
    }

    public void CutOffObject(GameObject tiedObject)
    {
        tiedObject.transform.parent = null;
    }
}
