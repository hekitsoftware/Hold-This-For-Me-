using UnityEngine;

public enum ItemType
{
    QuestItem,
    Weapon,
    Puzzle,
    Wearable
}

[System.Serializable]
public class Item
{
    public string itemID; //reference name
    public string itemName; //name said out loud
    public ItemType itemType;
    public Sprite itemSprite;
    public bool isEquipped;
    public string usedForID; // E.g., "door1", "dogDoor", etc.
    public bool hasPhysicalObject;

    public Item(string id, string name, ItemType type, bool hasPhysical, string usedFor = "", Sprite Sprite = null)
    {
        itemID = id;
        itemName = name;
        itemType = type;
        hasPhysicalObject = hasPhysical;
        usedForID = usedFor;
        isEquipped = false;
        itemSprite = Sprite;
    }
}
