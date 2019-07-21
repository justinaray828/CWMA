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
    public Animator animator;

    private float smoothTime = 0.3f;
    private float yVelocity = 0.3f;

    public float highestPanelLocation;
    private float lowestPanelLocation = -90f;
    private float panelLocation;

    /// <summary>
    /// Set to true to pop up panel and false to bring it down.
    /// </summary>
    public bool PopUp
    {
        get => animator.GetBool("IsOpen");
        set => animator.SetBool("IsOpen", value);
    }

    public void AdvanceDialogue(string speakerName, string speakerText)
    {
        if (speakerText.Length > 100)
            Debug.LogError("speakerText is over 100 characters");

        nameText.text = speakerName + ":";
        dialogueText.text = speakerText;
    }

    private void ToggleDialogueText(bool textState)
    {
        nameText.enabled = textState;
        dialogueText.enabled = textState;
    }

}