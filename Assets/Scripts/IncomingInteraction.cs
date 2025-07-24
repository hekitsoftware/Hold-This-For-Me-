using UnityEngine;

public class IncomingInteraction : MonoBehaviour
{
    //Thing
    public Interact activationAgent;

    private void OnEnable()
    {
        if (activationAgent != null)
        {
            activationAgent.GetInteractEvent.hasInteracted += InteractionEvent;
        }
    }

    private void OnDisable()
    {
        if(activationAgent != null)
        {
            activationAgent.GetInteractEvent.hasInteracted -= InteractionEvent;
        }
    }

    public void InteractionEvent()
    {
        Debug.Log("MINGLE");
    }
}
