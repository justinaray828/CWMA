using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles all dialogue functions
/// </summary>
public class DialogueScript : MonoBehaviour
{
    [SerializeField] RectTransform dialoguePanelRectTransform;

    [SerializeField] Text nameText;
    [SerializeField] Text dialogueText;

    [SerializeField] float panelTravelDistance = 0f;
    [SerializeField] float smoothTime = 0.3f;
    [SerializeField] float yVelocity = 0.3f;
    float highestPanelLocation = 74f;
    float lowestPanelLocation = -90f;

    public bool popUp = true;

    void Start()
    {

    }

    void FixedUpdate()
    {
        DialoguePanelPopUp(popUp);
    }

    void DialoguePanelPopUp(bool popUp)
    {
        float newPosition;

        if (popUp)
        {
            newPosition = Mathf.SmoothDamp(dialoguePanelRectTransform.position.y, highestPanelLocation, ref yVelocity, smoothTime);
            dialoguePanelRectTransform.position = new Vector2(dialoguePanelRectTransform.position.x, newPosition);
        }
        else
        {
            newPosition = Mathf.SmoothDamp(dialoguePanelRectTransform.position.y, lowestPanelLocation, ref yVelocity, smoothTime);
            dialoguePanelRectTransform.position = new Vector2(dialoguePanelRectTransform.position.x, newPosition);
        }
    }
}
