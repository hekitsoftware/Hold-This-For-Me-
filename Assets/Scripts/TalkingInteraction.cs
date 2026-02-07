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

}
