using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

    [SerializeField] private string itemName;
    [SerializeField] private int itemId;
    public string GetItemName()
    {
        return itemName;
    }
    public int GetItemId() 
    {
        return itemId; 
    }

}
