using System;
using UnityEngine;

public class TalkingManager : MonoBehaviour
{
    public event Action<string> onEnterTalking;

    private bool talkingPlaying = false;

    private void OnEnable()
    {
        GameManager.instance.talkingManager.onEnterTalking += EnterTalking;
    }

    private void OnDisable()
    {
        GameManager.instance.talkingManager.onEnterTalking -= EnterTalking;
    }

    public void EnterTalking(string knotName)
    {
        if (talkingPlaying) { return; }

        talkingPlaying = true;

        Debug.Log("Talking starting at: " + knotName);
        if (onEnterTalking != null)
        {
            onEnterTalking(knotName);
        }
    }
}
