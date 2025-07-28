using UnityEngine;
using UnityEngine.Events;

public class ItemChecker : MonoBehaviour
{
    [Tooltip("Item Needed (ID)")]
    public string requiredItemID;

    [Header("Events")]
    public UnityEvent onSuccess;
    public UnityEvent onFail;

    public void Try()
    {
        if (Inventory.Instance.HasItem(requiredItemID))
        {
            Debug.Log("Sucess w. " + requiredItemID);
            onSuccess?.Invoke();
        }
        else
        {
            Debug.Log("Failed: No " + requiredItemID);
            onFail?.Invoke();
        }
    }
}
