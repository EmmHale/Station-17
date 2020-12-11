/************************************
 * Author: Emmett Hale
 * 
 * Purpose: Interactable specalized for
 * key card doors
 ************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCardLockInteractable : Interactable
{
    public Item correctCard;

    float timeOpen = 0;
    public float timeToWait = 8f;
    public GameObject Door;
    Animator animator;
    AudioSource source;
    bool openedWithThis = false;



    public List<GameObject> additionalDoors;

    private void Start()
    { 
        if (Door != null)
        {
            animator = Door.GetComponent<Animator>();
        }
        else
        {
            Debug.Log("No door!");
        }
    }

    public void Update()
    {
        if (animator.GetBool("IsOpen") && openedWithThis)
        {
            timeOpen -= Time.deltaTime;

            if(timeOpen <= 0)
            {
                animator.SetBool("IsOpen", false);
                animator.SetBool("IsClosed", true);
                timeOpen = 0;
                openedWithThis = false;

                foreach(GameObject door in additionalDoors)
                {
                    door.GetComponent<Animator>().SetBool("IsOpen", false);
                    door.GetComponent<Animator>().SetBool("IsClosed", true);
                }
            }
        }
    }

    public override bool PreformAction()
    {
        if(willFail)
        {
            return false;
        }

        if(correctCard == null || (PlayerInventory.instance != null && PlayerInventory.instance.Search(correctCard)) && animator != null)
        {
            if(animator.GetBool("IsOpen"))
            {
                return false;
            }

            return true;
        }
        else
        {
            if(correctCard == null)
            {
                return true;
            }
            return false;
        }
    }

    public override void CompleteAction()
    {
        if(animator != null)
        {
            animator.SetBool("IsOpen", true);
            animator.SetBool("IsClosed", false);
            timeOpen = timeToWait;
            openedWithThis = true;

            foreach (GameObject door in additionalDoors)
            {
                door.GetComponent<Animator>().SetBool("IsOpen", true);
                door.GetComponent<Animator>().SetBool("IsClosed", false);
            }
        }
        else
        {
            Debug.Log("No animator!");
        }

        base.CompleteAction();
    }
}
