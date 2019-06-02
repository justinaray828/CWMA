using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles all dialogue functions
/// </summary>
public class DialoguePanelController : MonoBehaviour
{
    public RectTransform dialoguePanelRectTransform;

    public Text nameText;
    public Text dialogueText;

    public float smoothTime = 0.3f;
    public float yVelocity = 0.3f;

    float highestPanelLocation = 74f;
    float lowestPanelLocation = -90f;
    float panelLocation;
    /// <summary>
    /// Set to true to pop up panel and false to bring it down.
    /// </summary>
    private bool popUp = false;

    public bool PopUp
    {
        get
        {
            return popUp;
        }
        set
        {
            popUp = value;
            DialoguePanelPopUp(popUp);
        }
    }

    void Start()
    {
        panelLocation = lowestPanelLocation;
    }

    void FixedUpdate()
    {
        MovePanel();
    }

    public void AdvanceDialogue(string speakerName, string speakerText)
    {
        if(speakerText.Length > 100)
            Debug.LogError("speakerText is over 100 characters");

        nameText.text = speakerName + ":";
        dialogueText.text = speakerText;
    }

    /// <summary>
    /// Call to pop up window. True is up and false is down.
    /// </summary>
    /// <param name="popUp"></param>
    private void DialoguePanelPopUp(bool popUp)
    {
        if (popUp)
        {
            panelLocation = highestPanelLocation;
        }
        else
        {
            panelLocation = lowestPanelLocation;
        }
    }

    void MovePanel()
    {
        float newPosition;
        newPosition = Mathf.SmoothDamp(dialoguePanelRectTransform.position.y, panelLocation, ref yVelocity, smoothTime);
        dialoguePanelRectTransform.position = new Vector2(dialoguePanelRectTransform.position.x, newPosition);
    }

    void ToggleDialogueText(bool textState)
    {
        nameText.enabled = textState;
        dialogueText.enabled = textState;
    }

   
}
