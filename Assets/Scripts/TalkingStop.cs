using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkingStop : MonoBehaviour
{
    public List<FacialAnimation> CharacterList;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject character in GameObject.FindGameObjectsWithTag("Character"))
        {
            CharacterList.Add(character.GetComponent<FacialAnimation>());
        }
    }

    public void StopAllTalking()
    {
        foreach (FacialAnimation FA in CharacterList)
        {
            FA.StopTalking();
        }
    }
}
