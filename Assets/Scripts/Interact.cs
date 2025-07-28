using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class InteractUnityEvent : UnityEvent<PlayerMovement> { }

public class Interact : MonoBehaviour
{
    [Header("Events")]
    public InteractUnityEvent OnInteract;

    // Call this method when the player interacts with this object
    public void CallInteract(PlayerMovement player)
    {
        OnInteract?.Invoke(player);
    }
}
