using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] public string itemID;
    [SerializeField] public string itemName;
    [SerializeField] public ItemType itemType;
    [SerializeField] public bool hasPhysicalObject;
    [SerializeField] public string usedForID;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Item item = new Item(itemID, itemName, itemType, hasPhysicalObject, usedForID);
            Inventory.Instance.AddItem(item);

            if (hasPhysicalObject)
                Destroy(gameObject); // remove from scene
        }
    }
}
