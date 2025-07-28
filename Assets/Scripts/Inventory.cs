using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public bool CanGun = false;
    public bool HasGun = false;

    public static Inventory Instance;
    public List<Item> items = new List<Item>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddItem(Item newItem)
    {
        items.Add(newItem);

        // Auto-equip wearables
        if (newItem.itemType == ItemType.Wearable)
        {
            EquipItem(newItem.itemID);
        }

        Debug.Log($"Added {newItem.itemName} to inventory.");
    }

    public void EquipItem(string itemID)
    {
        Item item = items.Find(i => i.itemID == itemID);
        if (item != null && item.itemType == ItemType.Weapon || item.itemType == ItemType.Wearable)
        {
            item.isEquipped = true;
            Debug.Log($"Equipped {item.itemName}");
            // Add logic here to actually show weapon/collar, etc.
        }
    }

    public bool HasItem(string itemID)
    {
        return items.Exists(i => i.itemID == itemID);
    }

    public Item GetItem(string itemID)
    {
        return items.Find(i => i.itemID == itemID);
    }
}
