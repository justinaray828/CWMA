using UnityEngine;

[System.Serializable]
public class CharacterData
{
    public string name;

    [Header("Talk-sound data:")]
    [Range(1f, 2f)]
    public float talkingPitch = 1.4f;

}