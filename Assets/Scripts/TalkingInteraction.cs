using UnityEngine;

public class TalkingInteraction : MonoBehaviour
{
    [Header("Ink Settings")]
    [SerializeField] public TextAsset InkFile;

    [Header("Audio Clip (For Non-VA Characters)")]
    [SerializeField] public AudioClip _audioClip;

    public AudioSource _audioSource;
    public Interact incomingInteraction;
    public TalkManager tManager;
    public talkID talkID;

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
            tManager.LoadNonVaClip(_audioClip);
            tManager.LoadTalkID(talkID);
            tManager.LoadSpeakerSource(_audioSource);
        }
    }
}
