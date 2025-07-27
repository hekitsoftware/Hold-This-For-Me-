using UnityEngine;
using Ink.UnityIntegration;
using Ink.Runtime;

public class PathTRIGGER : MonoBehaviour
{
    public Animator anim;
    public TalkManager talk;

    private void Start()
    {
        anim.SetBool("IsUp", false);
    }

    public void ShowPath()
    {
        anim.SetBool("IsUp", true);
        talk.FinishTalking();
    }
}
