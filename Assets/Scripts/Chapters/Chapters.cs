using UnityEngine;

[CreateAssetMenu(fileName = "Chapters", menuName = "Scriptable Objects/Chapters")]
public class Chapters : ScriptableObject
{
    public string ChapterName;
    public int ChapterIndex;
    public GameObject gameObject;
}
