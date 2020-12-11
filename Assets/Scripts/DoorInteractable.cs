/************************************
 * Author: Emmett Hale
 * 
 * Purpose: Specialized door interaction
 * class
 ************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractable : Interactable
{
    Animator DoorAnimator;


    
    // Start is called before the first frame update
    void Start()
    {
        DoorAnimator = GetComponent<Animator>();
        interactText = "E: Open Door";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override bool PreformAction()
    {
        Debug.Log("Moving Door...");
        if (DoorAnimator.GetBool("IsOpen"))
        {

            DoorAnimator.SetBool("IsOpen", false);
            DoorAnimator.SetBool("IsClosed", true);
            interactText = "E: Open Door";
        }
        else
        {
            DoorAnimator.SetBool("IsOpen", true);
            DoorAnimator.SetBool("IsClosed", false);
            interactText = "E: Close Door";
        }

        return true;
    }

}
