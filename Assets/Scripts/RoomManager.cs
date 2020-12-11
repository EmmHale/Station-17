/************************************
 * Author: Emmett Hale
 * 
 * Purpose: Manages room data
 ************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoomManager : MonoBehaviour
{
    public static List<RoomManager> rooms;
    public List<Enemy> enemyList = new List<Enemy>();

    bool roomActive = false;

    public List<Interactable> managedInteractions = new List<Interactable>();

    public void Start()
    {
        //Enable singleton
        if(rooms != null)
        {
            rooms.Add(this);
        }
        else
        {
            rooms = new List<RoomManager>();
            rooms.Add(this);
        }
    }

    private float timeDeavtivateTrigger = 4;
    private bool deactivating = false;
    private float timeSince = 0;
    private void Update()
    {
        //Delay turn enemies off for timeDeavtivateTrigger
        if (deactivating)
        {
            if(timeSince >= timeDeavtivateTrigger)
            {
                Debug.Log("Deactivating");
                foreach (Enemy enemy in enemyList)
                {
                    enemy.DeactivateEnemy();
                    enemy.gameObject.GetComponent<NavMeshAgent>().isStopped = true;
                    enemy.gameObject.GetComponent<NavMeshAgent>().ResetPath();
                    enemy.gameObject.GetComponent<NavMeshAgent>().Warp(enemy.defaultPosition);
                    enemy.transform.rotation = enemy.defaultRotation;
                }
                deactivating = false;
                timeSince = 0;
            }
            else
            {
                timeSince += Time.deltaTime;
            }
        }
    }
    public void ActivateRoom()
    {
        if(!roomActive)
        {
            roomActive = true;

            //Activate enemyList
            foreach(Enemy enemy in enemyList)
            {
                enemy.ActivateEnemy();
            }
            //Activate Triggers?

            deactivating = false;
            timeSince = 0;
        }
    }

    //Turn room off
    public void DeactivateRoom()
    {
        if(roomActive)
        {
            roomActive = false;

            deactivating = true;

            foreach (Enemy enemy in enemyList)
            {
                enemy.DeactivateEnemy();
                enemy.GetComponent<NavMeshAgent>().isStopped = true;
            }
        }
    }

    //Save a rooms progression
    public void SaveRoom()
    {
        foreach (Interactable item in managedInteractions)
        {
            if (item.completed && !item.saved)
            {
                item.saved = true;
            }
        }
    }

    //Reset unsaved progress
    public void PlayerDead()
    {
        foreach (Interactable item in managedInteractions)
        {
            if (item.completed && !item.saved)
            {
                item.PlayerDied();
            }
        }
    }
}
