using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory instance;
    public List<Item> items = new List<Item>();

    public void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Two inventory instance!");
            return;
        }

        instance = this;
    }


    public void Add(Item item)
    {
        items.Add(item);
    }

    public void Remove(Item item)
    {
        items.Remove(item);
    }

    public bool Search(Item item)
    {
        bool found = false;
        foreach(Item stuff in items)
        {
            if(stuff.name == item.name)
            {
                found = true;
            }
        }

        return found;
    }
}
