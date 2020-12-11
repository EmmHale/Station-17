/************************************
 * Author: Emmett Hale
 * 
 * Purpose: This script sets player 
 * checkpoint when added to a collider
 * and saves rooms accordingly
 * 
 * -> Will expand with bigger save system
 ************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if(PlayerMovement.instance != null)
        {
            PlayerMovement.instance.currentCheckpoint = transform.position;
            PlayerMovement.instance.currentCheckpointRotation = transform.rotation;

            if (RoomManager.rooms != null)
            {
                Debug.Log("Saving Rooms");
                foreach(RoomManager room in RoomManager.rooms)
                {
                    room.SaveRoom();
                }
            }
        }
    }

}
