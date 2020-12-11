/****************************************
 * Author: Emmett Hale
 * 
 * Date Created: A month in 2020
 * 
 * Purpose: Main enemy script
 ****************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    bool active = false;

    [Tooltip("Radius enemy checks for player to move")]
    public float lookRadius = 10f;

    [Tooltip("Range to check for jump")]
    public float jumpRange = 1f;

    [Tooltip("Seconds between movements")]
    public float timeBetweenMoves = 1f;

    [Tooltip("Time spent moving")]
    public float timeMoving = .5f;

    NavMeshAgent agent;
    float timeSinceMovement = 0f;
    float timeSinceStartMoving = 0f;
    bool isMoving = false;

    [Tooltip("Audio Source of movements")]
    public AudioSource source;

    public List<AudioClip> walkClips = new List<AudioClip>();

    public Vector3 defaultPosition;
    public Quaternion defaultRotation;

   

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        defaultPosition = transform.position;
        defaultRotation = transform.rotation;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (PlayerMovement.instance != null && other.tag == "Player")
        {
            if (AudioManager.instance != null)
            {
                AudioManager.instance.Play("Enemy Kill 2");
            }
            PlayerMovement.instance.Kill();
            Debug.Log("Killing player");
        }
        else if(other.tag == "Door")
        {
            if(other.gameObject.GetComponent<Animator>().GetBool("IsClosed"))
            {
                other.gameObject.GetComponent<Animator>().SetBool("IsOpen", true);
                other.gameObject.GetComponent<Animator>().SetBool("IsClosed", false);
            }
        }
    }

    public void ActivateEnemy()
    {
        if(!active)
        {
            active = true;
        }
    }

    public void DeactivateEnemy()
    {
        if (active)
        {
            active = false;

        }
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
        Gizmos.DrawWireSphere(transform.position, jumpRange);
    }

    float distance = 0f;

    public void Update()
    {
        distance = Vector3.Distance(PlayerInteract.instance.transform.position, transform.position);

        if (active && distance <= lookRadius)
        {
            //Check for player proximity

            if (PlayerInteract.instance.IsDoingLongAction())
            {
                //If the enemy is not moving
                if (!isMoving)
                {
                    if (timeSinceMovement >= timeBetweenMoves)
                    {
                        StartMoving();
                    }
                    else
                    {
                        timeSinceMovement += Time.deltaTime;
                    }
                }
                else
                {
                    if(timeSinceStartMoving >= timeMoving)
                    {
                        StopMoving();
                    }
                    else
                    {
                        timeSinceStartMoving += Time.deltaTime;
                    }
                }

            }
            else
            {
                StopMoving();

                if (distance <= jumpRange)
                {
                    //jump
                }
            }
        }
    }

    public void StopMoving()
    {
        isMoving = false;
        agent.isStopped = true;
        timeSinceMovement = 0;

        if (source)
        {
            source.Stop();
        }
    }

    public void StartMoving()
    {
        isMoving = true;
        agent.isStopped = false;
        timeSinceMovement = 0;
        timeSinceStartMoving = 0;

        NavMeshPath path = new NavMeshPath();
        if (agent.CalculatePath(PlayerInteract.instance.transform.position, path))
        {
            agent.SetDestination(PlayerInteract.instance.transform.position);
            if (source)
            {
                source.clip = walkClips[Random.Range(0, walkClips.Count - 1)];
                source.Play();
            }
        }

        
    }
}
