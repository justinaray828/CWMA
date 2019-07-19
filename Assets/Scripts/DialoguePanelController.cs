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
    //public Animator animator;

    private float smoothTime = 0.3f;
    private float yVelocity = 0.3f;

    public float highestPanelLocation;
    private float lowestPanelLocation = -90f;
    private float panelLocation;

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

    private void Start()
    {
        panelLocation = lowestPanelLocation;
        //OpenDialoguePanel();
    }

    private void FixedUpdate()
    {
        MovePanel();
    }

    public void AdvanceDialogue(string speakerName, string speakerText)
    {
        if (speakerText.Length > 100)
            Debug.LogError("speakerText is over 100 characters");

        nameText.text = speakerName + ":";
        dialogueText.text = speakerText;
    }

    public void OpenDialoguePanel()
    {
        //animator.SetBool("IsOpen", true);
    }

    public void CloseDialoguePanel()
    {
       // animator.SetBool("IsOpen", false);
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

    private void MovePanel()
    {
        float newPosition;
        newPosition = Mathf.SmoothDamp(dialoguePanelRectTransform.position.y, panelLocation, ref yVelocity, smoothTime);
        dialoguePanelRectTransform.position = new Vector2(dialoguePanelRectTransform.position.x, newPosition);
    }

    private void ToggleDialogueText(bool textState)
    {
        nameText.enabled = textState;
        dialogueText.enabled = textState;
    }

}