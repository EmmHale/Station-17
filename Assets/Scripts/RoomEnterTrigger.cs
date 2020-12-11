/************************************
 * Author: Emmett Hale
 * 
 * Purpose: Trigger for room managment
 ************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEnterTrigger : MonoBehaviour
{
    // Start is called before the first frame update

    public RoomManager roomToEnter;
    public RoomManager roomToLeave;
    public void OnTriggerEnter(Collider other)
    {
        if(roomToEnter != null)
        {
            roomToEnter.ActivateRoom();
            if(PlayerMovement.instance != null)
            {
                PlayerMovement.instance.currentRoom = roomToEnter;
            }
        }

        if (roomToLeave != null)
        {
            roomToLeave.DeactivateRoom();
        }

    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
