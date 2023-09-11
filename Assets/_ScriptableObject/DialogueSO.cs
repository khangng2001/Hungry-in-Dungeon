using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "ScriptableObjects/DialogueSO")]
public class DialogueSO : ScriptableObject
{
    public string name;
    public Sprite face;
    public string[] sentences;
}
