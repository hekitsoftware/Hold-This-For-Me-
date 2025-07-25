using UnityEngine;

public class TalkingInteraction : MonoBehaviour
{
    [Header("Ink Settings")]
    [SerializeField] public TextAsset InkFile;

    public Interact incomingInteraction;
    public TalkManager tManager;

    private void OnEnable()
    {
        if (incomingInteraction != null)
        {
            incomingInteraction.GetInteractEvent.hasInteracted += InteractionEvent;
        }
    }

    private void OnDisable()
    {
        if (incomingInteraction != null)
        {
            incomingInteraction.GetInteractEvent.hasInteracted -= InteractionEvent;
        }
    }

    public void InteractionEvent()
    {
        Debug.Log("Interacted with " + this);
        if (tManager != null && InkFile != null)
        {
            tManager.LoadNewInk(InkFile);
        }
    }
}
