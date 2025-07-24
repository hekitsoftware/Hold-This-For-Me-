using UnityEngine;

public class IncomingInteraction : MonoBehaviour
{
    //Thing
    [Header("Talking (OPTIONAL)")]
    [SerializeField] private string talkingKnotName;


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
        if (!talkingKnotName.Equals(""))
        {
            GameManager.instance.talkingManager.EnterTalking(talkingKnotName);
        }
    }
}
