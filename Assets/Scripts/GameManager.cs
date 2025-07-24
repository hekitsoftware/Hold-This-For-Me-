using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public TalkingManager talkingManager;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one GameManager in Scene");
        }
        instance = this;

        talkingManager = new TalkingManager();
    }
}
