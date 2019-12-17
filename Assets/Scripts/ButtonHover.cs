using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler
{
    private AudioEventHandler aeh;

    public void Start()
    {
        aeh = FindObjectOfType<EventManager>().GetComponent<AudioEventHandler>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        aeh.ButtonHover();
    }
}
