using UnityEngine;

public enum typeID
{
    Silent,
    NoVA,
    VA,
}

[CreateAssetMenu(fileName = "talkID", menuName = "ScriptableObjects/Talk ID")]
public class talkID : ScriptableObject
{
    public typeID typeID;
}
