using UnityEngine;

public class Interact : MonoBehaviour
{
    private InteractEvent interact = new InteractEvent();
    private PlayerMovement player;

    public InteractEvent GetInteractEvent
    {
        get
        {
            if (interact == null) interact = new InteractEvent();
            return interact;
        }
    }

    public PlayerMovement getPlayer => player;

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
