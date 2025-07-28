using UnityEngine;

public class TalkingInteraction : MonoBehaviour
{
    [Header("Ink Settings")]
    public TextAsset InkFile;
    private string startKnot;

    [Header("Audio Clip (For Non-VA Characters)")]
    public AudioClip audioClip;

    public AudioSource audioSource;
    public TalkManager talkManager;
    public talkID talkID;

    //For normal talking
    public void SetStartKnot(string knot)
    {
        startKnot = knot;
    }

    public void TriggerDialogueWithPlayer(PlayerMovement player)
    {
        if (talkManager != null && InkFile != null)
        {
            talkManager.LoadInkAtKnot(InkFile, startKnot);
            talkManager.LoadNonVaClip(audioClip);
            talkManager.LoadTalkID(talkID);
            talkManager.LoadSpeakerSource(audioSource);
            player.ZoomInTalking();
        }
    }

}
