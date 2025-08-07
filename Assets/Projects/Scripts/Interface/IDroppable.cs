using System.Collections.Generic;
using UnityEngine;

public interface IDroppable
{
    List<DropItemData> DropItems { get; } 
    void Drop(); 
}

[System.Serializable]
public class DropItemData
{
    public GameObject itemPrefab; 
    public int amount;            

    public DropItemData(GameObject prefab, int amount)
    {
        this.itemPrefab = prefab;
        this.amount = amount;
    }
}