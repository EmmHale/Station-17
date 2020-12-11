/************************************
 * Author: Emmett Hale
 * 
 * Purpose: Item interactable
 ************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteractable : Interactable
{
    public Item item;
    public override bool PreformAction()
    {
        base.PreformAction();

        if(PlayerInventory.instance != null)
        {
            PlayerInventory.instance.Add(item);
            gameObject.SetActive(false);
        }

        return true;
    }
}
