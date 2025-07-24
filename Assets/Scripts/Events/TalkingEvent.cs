using System;
using UnityEngine;

public class TalkingEvent
{
    public event Action<string> onEnterDialogue;
    public void EnterDialogue(string knotName)
    {
        onEnterDialogue?.Invoke(knotName);
    }
}
