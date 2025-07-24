using System.Xml.Serialization;
using UnityEngine;

public class Interact : MonoBehaviour
{
    InteractEvent interact = new InteractEvent();
    PlayerMovement player;

    public InteractEvent GetInteractEvent
    {
        get
        {
            if (interact == null) interact = new InteractEvent();
            return interact;
        }
    }

    public PlayerMovement getPlayer
    {
        get
        {
            return player;
        }
    }

    public void CallInteract(PlayerMovement interactedPlayer)
    {
        player = interactedPlayer;
        interact.CallIntactEvent();
    }
}

public class InteractEvent
{
    public delegate void InteractHandler();

    public event InteractHandler hasInteracted;

    public void CallIntactEvent() => hasInteracted?.Invoke();
}